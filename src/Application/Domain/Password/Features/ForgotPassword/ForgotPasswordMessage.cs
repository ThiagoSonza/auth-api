namespace Application.Domain.Password.Features.ForgotPassword;

public record ForgotPasswordMessage(string UserId, string Name, string Email, string Token);