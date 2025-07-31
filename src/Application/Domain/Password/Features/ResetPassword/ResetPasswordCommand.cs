using Microsoft.AspNetCore.Identity.Data;
using SharedKernel;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.Password.Features.ResetPassword;

public class ResetPasswordCommand : IRequest<Result>
{
    private ResetPasswordCommand(string email, string token, string resetCode, string newPassword)
    {
        Email = email;
        Token = token;
        ResetCode = resetCode;
        NewPassword = newPassword;
    }

    public string Email { get; }
    public string Token { get; }
    public string ResetCode { get; }
    public string NewPassword { get; }

    public static implicit operator ResetPasswordCommand(ResetPasswordRequest request)
    {
        return new ResetPasswordCommand(request.Email, string.Empty, request.ResetCode, request.NewPassword);
    }
}
