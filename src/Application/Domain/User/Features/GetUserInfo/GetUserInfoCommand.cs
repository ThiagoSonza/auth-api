using System.Security.Claims;
using SharedKernel;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.User.Features.GetUserInfo;

public class GetUserInfoCommand : IRequest<Result<GetUserInfoResponse>>
{
    private GetUserInfoCommand(string userId)
    {
        UserId = userId;
    }

    public string UserId { get; }

    public static implicit operator GetUserInfoCommand(ClaimsIdentity identity)
    {
        string userId = identity.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        return new GetUserInfoCommand(userId);
    }
}
