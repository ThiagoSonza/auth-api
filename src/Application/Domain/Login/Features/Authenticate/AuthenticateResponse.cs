namespace Application.Domain.Login.Features.Authenticate;

public record AuthenticateResponse(string AccessToken, string RefreshToken);
