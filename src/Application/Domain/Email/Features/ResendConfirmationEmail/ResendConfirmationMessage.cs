namespace Application.Domain.Email.Features.ResendConfirmationEmail;

public record ResendConfirmationMessage(string UserId, string Name, string Email, string Token);
