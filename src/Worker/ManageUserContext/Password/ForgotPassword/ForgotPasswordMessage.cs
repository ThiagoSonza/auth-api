namespace Worker.ManageUserContext.Password.ForgotPassword;

public record ForgotPasswordMessage(string UserId, string Name, string Email, string Token);