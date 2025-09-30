namespace Worker.ManageUserContext.Password.ForgotPassword;

public record ForgotPasswordMessage(string UserId, string UserName, string Email, string Token);