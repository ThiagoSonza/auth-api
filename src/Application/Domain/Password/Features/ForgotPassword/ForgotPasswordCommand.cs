using Microsoft.AspNetCore.Identity.Data;
using SharedKernel;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.Password.Features.ForgotPassword;

public class ForgotPasswordCommand : IRequest<Result<string>>
{
    private ForgotPasswordCommand(string email)
    {
        Email = email;
    }

    public string Email { get; } = string.Empty;

    public static implicit operator ForgotPasswordCommand(ForgotPasswordRequest request)
    {
        return new ForgotPasswordCommand(request.Email);
    }
}
