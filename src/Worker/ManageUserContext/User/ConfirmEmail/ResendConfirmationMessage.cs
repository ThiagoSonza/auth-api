namespace Worker.ManageUserContext.User.ConfirmEmail;

public record ResendConfirmationMessage(string UserId, string Name, string Email, string Token);