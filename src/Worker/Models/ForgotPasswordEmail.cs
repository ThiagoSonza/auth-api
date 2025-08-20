namespace Worker.Models;

public record ForgotPasswordEmail(string UserId, string UserName, string Email, string Token);