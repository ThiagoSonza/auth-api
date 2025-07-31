using Domain.User;

namespace Application.Domain.User.Features.GetUserInfo;

public record GetUserInfoResponse
{
    private GetUserInfoResponse(string id, string userName, string email, string? phoneNumber)
    {
        Id = id;
        UserName = userName;
        Email = email;
        PhoneNumber = phoneNumber;
    }

    public string Id { get; init; } = string.Empty;
    public string UserName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string? PhoneNumber { get; init; }

    public static implicit operator GetUserInfoResponse(UserDomain user)
    {
        return new GetUserInfoResponse(user.Id, user.UserName!, user.Email!, user.PhoneNumber);
    }
}