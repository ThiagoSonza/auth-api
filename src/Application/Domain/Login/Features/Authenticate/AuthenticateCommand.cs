using Microsoft.AspNetCore.Identity.Data;
using SharedKernel;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.Login.Features.Authenticate;

public class AuthenticateCommand : IRequest<Result<AuthenticateResponse>>
{
    private AuthenticateCommand(string email, string password, string? twoFactorCode = null, string? twoFactorRecoveryCode = null)
    {
        Email = email;
        Password = password;
        TwoFactorCode = twoFactorCode;
        TwoFactorRecoveryCode = twoFactorRecoveryCode;
    }

    public string Email { get; } = string.Empty;
    public string Password { get; } = string.Empty;
    public string? TwoFactorCode { get; }
    public string? TwoFactorRecoveryCode { get; }

    public static implicit operator AuthenticateCommand(LoginRequest request)
    {
        return new AuthenticateCommand(request.Email, request.Password, request.TwoFactorCode, request.TwoFactorRecoveryCode);
    }
}
