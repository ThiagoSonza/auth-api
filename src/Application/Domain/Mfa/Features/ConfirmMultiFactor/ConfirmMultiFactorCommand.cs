using System.Security.Claims;
using SharedKernel;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.Mfa.Features.ConfirmMultiFactor;

public class ConfirmMultiFactorCommand : IRequest<Result<string>>
{
    private ConfirmMultiFactorCommand(string code, string userId)
    {
        Code = code;
        UserId = userId;
    }

    public string Code { get; }
    public string UserId { get; }

    public static ConfirmMultiFactorCommand Create(string code, ClaimsPrincipal userPrincipal)
    {
        var userId = userPrincipal.FindFirstValue(ClaimTypes.NameIdentifier)!;
        return new ConfirmMultiFactorCommand(code, userId);
    }
}
