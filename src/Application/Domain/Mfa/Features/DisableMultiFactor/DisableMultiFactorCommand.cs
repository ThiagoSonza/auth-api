using System.Security.Claims;
using SharedKernel;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.Mfa.Features.DisableMultiFactor;

public record DisableMultiFactorCommand : IRequest<Result<string>>
{
    private DisableMultiFactorCommand(string userId)
    {
        UserId = userId;
    }

    public string UserId { get; }

    public static DisableMultiFactorCommand Create(ClaimsPrincipal userPrincipal)
    {
        var userId = userPrincipal.FindFirstValue(ClaimTypes.NameIdentifier)!;
        return new DisableMultiFactorCommand(userId);
    }
}
