using SharedKernel;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.User.Features.RegisterUser;

public class RegisterUserCommand : IRequest<Result<RegisterUserResponse>>
{
    private RegisterUserCommand(string email, string password, string name)
    {
        Email = email;
        Password = password;
        Name = name;
    }

    public string Email { get; } = string.Empty;
    public string Password { get; } = string.Empty;
    public string Name { get; } = string.Empty;

    public static implicit operator RegisterUserCommand(RegisterUserRequest request)
    {
        return new RegisterUserCommand(request.Email, request.Password, request.Name);
    }
}
