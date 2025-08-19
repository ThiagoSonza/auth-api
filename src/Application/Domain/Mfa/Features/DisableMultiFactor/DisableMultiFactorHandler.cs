using Domain.User;
using Microsoft.AspNetCore.Identity;
using SharedKernel;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.Mfa.Features.DisableMultiFactor;

public class DisableMultiFactorHandler(
    UserManager<UserDomain> userManager,
    DisableMultiFactorTelemetry telemetry
) : IRequestHandler<DisableMultiFactorCommand, Result<string>>
{
    public async Task<Result<string>> Handle(DisableMultiFactorCommand command, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(command.UserId);
        if (user is null)
        {
            telemetry.MarkUserNotFound(command.UserId);
            return Result.Failure<string>("Usuário não encontrado");
        }

        await userManager.SetTwoFactorEnabledAsync(user, false);
        telemetry.MarkTwoFactorDisable(user.Id);
        return Result.Success("2FA disabled successfully.");
    }
}
