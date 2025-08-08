using Microsoft.AspNetCore.Identity.Data;
using SharedKernel;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.User.Features.RegisterUser;

public class RegisterUserCommand : IRequest<Result<RegisterUserResponse>>
{
    private RegisterUserCommand(string email, string password)
    {
        Email = email;
        Password = password;
    }

    public string Email { get; } = string.Empty;
    public string Password { get; } = string.Empty;

    public static implicit operator RegisterUserCommand(RegisterRequest request)
    {
        return new RegisterUserCommand(request.Email, request.Password);
    }
}
