using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SharedKernel;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.Login.Features.RefreshToken;

public class RefreshTokenHandler(
    UserManager<UserDomain> userManager,
    IConfiguration config) : IRequestHandler<RefreshTokenCommand, Result<RefreshTokenResponse>>
{
    public async Task<Result<RefreshTokenResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var user = (await userManager.FindByIdAsync(request.UserId))!;

        List<string> errors = [];
        if (await userManager.IsLockedOutAsync(user))
            errors.Add("Essa conta está bloqueada");
        else if (!await userManager.IsEmailConfirmedAsync(user))
            errors.Add("Essa conta precisa confirmar seu e-mail antes de realizar o login");

        if (errors.Count > 0)
            return Result.Failure<RefreshTokenResponse>(errors);

        return Result.Success(await GerarCredenciais(user.Email!));
    }

    private async Task<RefreshTokenResponse> GerarCredenciais(string email)
    {
        var user = (await userManager.FindByEmailAsync(email))!;
        var accessTokenClaims = await ObterClaims(user, adicionarClaimsUsuario: true);
        var refreshTokenClaims = await ObterClaims(user, adicionarClaimsUsuario: false);

        var dataExpiracaoAccessToken = DateTime.Now.AddSeconds(60 * 60);
        var dataExpiracaoRefreshToken = DateTime.Now.AddSeconds(60 * 60 * 2);

        var accessToken = GerarToken(accessTokenClaims, dataExpiracaoAccessToken);
        var refreshToken = GerarToken(refreshTokenClaims, dataExpiracaoRefreshToken);

        return new RefreshTokenResponse(AccessToken: accessToken, RefreshToken: refreshToken);
    }

    private string GerarToken(IEnumerable<Claim> claims, DateTime dataExpiracao)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var jwt = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: claims,
            notBefore: DateTime.Now,
            expires: dataExpiracao,
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    private async Task<IList<Claim>> ObterClaims(UserDomain user, bool adicionarClaimsUsuario)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Nbf, DateTime.Now.ToString()),
            new(JwtRegisteredClaimNames.Iat, DateTimeOffset.Now.ToUnixTimeSeconds().ToString())
        };

        if (adicionarClaimsUsuario)
        {
            var userClaims = await userManager.GetClaimsAsync(user);
            var roles = await userManager.GetRolesAsync(user);

            claims.AddRange(userClaims);

            foreach (var role in roles)
                claims.Add(new Claim("role", role));
        }

        return claims;
    }
}