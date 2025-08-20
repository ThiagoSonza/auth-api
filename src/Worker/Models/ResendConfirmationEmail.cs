namespace Worker.Models;

public record ResendConfirmationEmail(string UserId, string Username, string Email, string Token);