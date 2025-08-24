using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SharedKernel;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.Login.Features.Authenticate;

public class AuthenticateHandler(
    UserManager<UserDomain> userManager,
    SignInManager<UserDomain> signInManager,
    AuthenticateTelemetry telemetry,
    IConfiguration config) : IRequestHandler<AuthenticateCommand, Result<AuthenticateResponse>>
{
    public async Task<Result<AuthenticateResponse>> Handle(AuthenticateCommand command, CancellationToken cancellationToken)
    {
        var result = await signInManager.PasswordSignInAsync(command.Email, command.Password, false, false);
        if (result.Succeeded)
        {
            var user = (await userManager.FindByEmailAsync(command.Email))!;
            if (!await userManager.IsEmailConfirmedAsync(user))
                return Result.Failure<AuthenticateResponse>("Essa conta precisa confirmar seu e-mail antes de realizar o login");

            var credentials = await GerarCredenciais(command.Email);
            telemetry.AuthenticateSuccessful(command.Email);
            return Result.Success(credentials);
        }

        var errors = GetAccountErrors(result);
        telemetry.AuthenticateFailed(command.Email, errors);
        return Result.Failure<AuthenticateResponse>(errors);
    }

    private static IEnumerable<string> GetAccountErrors(SignInResult result)
    {
        if (result.IsLockedOut)
            yield return "Essa conta está bloqueada";
        if (result.IsNotAllowed)
            yield return "Essa conta não tem permissão para fazer login";
        if (result.RequiresTwoFactor)
            yield return "É necessário confirmar o login no seu segundo fator de autenticação";
        if (!result.Succeeded)
            yield return "Usuário ou senha estão incorretos";
    }

    private async Task<AuthenticateResponse> GerarCredenciais(string email)
    {
        var user = (await userManager.FindByEmailAsync(email))!;
        var accessTokenClaims = await ObterClaims(user, adicionarClaimsUsuario: true);
        var refreshTokenClaims = await ObterClaims(user, adicionarClaimsUsuario: false);

        var dataExpiracaoAccessToken = DateTime.Now.AddSeconds(60 * 60);
        var dataExpiracaoRefreshToken = DateTime.Now.AddSeconds(60 * 60 * 2);

        var accessToken = GerarToken(accessTokenClaims, dataExpiracaoAccessToken);
        var refreshToken = GerarToken(refreshTokenClaims, dataExpiracaoRefreshToken);

        return new AuthenticateResponse(AccessToken: accessToken, RefreshToken: refreshToken);
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