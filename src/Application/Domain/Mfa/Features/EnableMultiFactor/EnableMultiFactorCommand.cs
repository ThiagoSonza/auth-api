using System.Security.Claims;
using SharedKernel;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.Mfa.Features.EnableMultiFactor;

public record EnableMultiFactorCommand : IRequest<Result>
{
    private EnableMultiFactorCommand(string userId)
    {
        UserId = userId;
    }

    public string UserId { get; }

    public static EnableMultiFactorCommand Create(ClaimsPrincipal userPrincipal)
    {
        var userId = userPrincipal.FindFirstValue(ClaimTypes.NameIdentifier)!;
        return new EnableMultiFactorCommand(userId);
    }
}
