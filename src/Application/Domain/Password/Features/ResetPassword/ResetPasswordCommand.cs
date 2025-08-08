using Microsoft.AspNetCore.Identity.Data;
using SharedKernel;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.Password.Features.ResetPassword;

public class ResetPasswordCommand : IRequest<Result<string>>
{
    private ResetPasswordCommand(string email, string resetCode, string newPassword)
    {
        Email = email;
        ResetCode = resetCode;
        NewPassword = newPassword;
    }

    public string Email { get; }
    public string ResetCode { get; }
    public string NewPassword { get; }

    public static implicit operator ResetPasswordCommand(ResetPasswordRequest request)
    {
        return new ResetPasswordCommand(request.Email, request.ResetCode, request.NewPassword);
    }
}
