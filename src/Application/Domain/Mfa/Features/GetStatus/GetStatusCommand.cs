using System.Security.Claims;
using SharedKernel;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.Mfa.Features.GetStatus;

public class GetStatusCommand : IRequest<Result<bool>>
{
    private GetStatusCommand(string userId)
    {
        UserId = userId;
    }

    public string UserId { get; }

    public static GetStatusCommand Create(ClaimsPrincipal claimsPrincipal)
    {
        var userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        return new GetStatusCommand(userId);
    }
}
