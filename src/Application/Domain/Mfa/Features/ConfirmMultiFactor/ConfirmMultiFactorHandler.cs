using Domain.User;
using Microsoft.AspNetCore.Identity;
using SharedKernel;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.Mfa.Features.ConfirmMultiFactor;

public class ConfirmMultiFactorHandler(UserManager<UserDomain> userManager)
    : IRequestHandler<ConfirmMultiFactorCommand, Result<string>>
{
    public async Task<Result<string>> Handle(ConfirmMultiFactorCommand command, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(command.UserId);
        if (user is null)
            return Result.Failure<string>("User not found.");

        var isValid = await userManager.VerifyTwoFactorTokenAsync(
            user,
            TokenOptions.DefaultAuthenticatorProvider,
            command.Code
        );

        if (!isValid)
            return Result.Failure<string>("Invalid code.");

        return Result.Success("Code validated with success");
    }
}