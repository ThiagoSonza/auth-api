namespace Application.Domain.Password.Features.ForgotPassword;

public record ForgotPasswordMessage(string UserId, string UserName, string Email, string Token);