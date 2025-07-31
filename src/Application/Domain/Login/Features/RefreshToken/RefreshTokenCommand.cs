using System.Security.Claims;
using SharedKernel;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.Login.Features.RefreshToken;

public class RefreshTokenCommand : IRequest<Result<RefreshTokenResponse>>
{
    private RefreshTokenCommand(string userId)
    {
        UserId = userId;
    }

    public string UserId { get; }

    public static implicit operator RefreshTokenCommand(ClaimsIdentity identity)
    {
        string name = identity?.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
        return new RefreshTokenCommand(name);
    }
}
