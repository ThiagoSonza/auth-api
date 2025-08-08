using Domain.User;

namespace Application.Domain.User.Features.GetUserInfo;

public record GetUserInfoResponse
{
    private GetUserInfoResponse(string id, string userName, string email, bool emailConfirmed, bool twoFactorEnabled)
    {
        Id = id;
        UserName = userName;
        Email = email;
        EmailConfirmed = emailConfirmed;
        TwoFactorEnabled = twoFactorEnabled;
    }

    public string Id { get; init; } = string.Empty;
    public string UserName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public bool EmailConfirmed { get; init; }
    public bool TwoFactorEnabled { get; init; }

    public static implicit operator GetUserInfoResponse(UserDomain user)
    {
        return new GetUserInfoResponse(user.Id, user.UserName!, user.Email!,
                user.EmailConfirmed, user.TwoFactorEnabled);
    }
}