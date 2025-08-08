using Domain.User;
using Microsoft.AspNetCore.Identity;
using SharedKernel;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.Mfa.Features.DisableMultiFactor;

public class DisableMultiFactorHandler(
    UserManager<UserDomain> userManager
) : IRequestHandler<DisableMultiFactorCommand, Result<string>>
{
    public async Task<Result<string>> Handle(DisableMultiFactorCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.UserId);
        if (user is null)
            return Result.Failure<string>("Usuário não encontrado");

        await userManager.SetTwoFactorEnabledAsync(user, false);
        return Result.Success("2FA disabled successfully.");
    }
}
