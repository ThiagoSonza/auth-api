using System.Security.Claims;
using SharedKernel;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.Mfa.Features.GenerateQrCode;

public class GenerateQrCodeCommand : IRequest<Result>
{
    private GenerateQrCodeCommand(string userId)
    {
        UserId = userId;
    }

    public string UserId { get; }

    public static GenerateQrCodeCommand Create(ClaimsPrincipal userPrincipal)
    {
        var userId = userPrincipal.FindFirstValue(ClaimTypes.NameIdentifier)!;
        return new GenerateQrCodeCommand(userId);
    }
}
