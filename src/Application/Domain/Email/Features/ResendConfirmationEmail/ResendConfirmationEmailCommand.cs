using Microsoft.AspNetCore.Identity.Data;
using SharedKernel;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.Email.Features.ResendConfirmationEmail;

public record ResendConfirmationEmailCommand : IRequest<Result>
{
    private ResendConfirmationEmailCommand(string email)
    {
        Email = email;
    }

    public string Email { get; } = string.Empty;

    public static implicit operator ResendConfirmationEmailCommand(ResendConfirmationEmailRequest request)
    {
        return new ResendConfirmationEmailCommand(request.Email);
    }
}
