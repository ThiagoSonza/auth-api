namespace Application.Domain.User.Features.RegisterUser;

public record RegisterUserRequest(string Email, string Password, string Name);