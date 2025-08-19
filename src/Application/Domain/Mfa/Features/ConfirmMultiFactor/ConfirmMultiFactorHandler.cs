using Domain.User;
using Microsoft.AspNetCore.Identity;
using SharedKernel;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.Mfa.Features.ConfirmMultiFactor;

public class ConfirmMultiFactorHandler(
    UserManager<UserDomain> userManager,
    ConfirmMultiFactorTelemetry telemetry)
    : IRequestHandler<ConfirmMultiFactorCommand, Result<string>>
{
    public async Task<Result<string>> Handle(ConfirmMultiFactorCommand command, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(command.UserId);
        if (user is null)
        {
            telemetry.MarkUserNotFound(command.UserId);
            return Result.Failure<string>("User not found.");
        }

        telemetry.MarkUserFound(command.UserId);

        var isValid = await userManager.VerifyTwoFactorTokenAsync(
            user,
            TokenOptions.DefaultAuthenticatorProvider,
            command.Code
        );

        if (!isValid)
        {
            telemetry.MarkInvalidCode(command);
            return Result.Failure<string>("Invalid code.");
        }

        telemetry.MarkCodeValidated(command.UserId);
        return Result.Success("Code validated with success");
    }
}