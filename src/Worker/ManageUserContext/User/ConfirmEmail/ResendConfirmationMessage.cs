namespace Worker.ManageUserContext.User.ConfirmEmail;

public record ResendConfirmationMessage(string UserId, string Username, string Email, string Token);