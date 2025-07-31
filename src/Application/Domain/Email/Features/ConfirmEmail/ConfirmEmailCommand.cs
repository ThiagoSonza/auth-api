using SharedKernel;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.Email.Features.ConfirmEmail;

public class ConfirmEmailCommand : IRequest<Result>
{
    private ConfirmEmailCommand(string token, string email)
    {
        Token = token;
        Email = email;
    }

    public string Token { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public static ConfirmEmailCommand Criar(string token, string email)
    {
        return new ConfirmEmailCommand(token, email);
    }
}
