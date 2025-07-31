using Domain.User;
using Microsoft.AspNetCore.Identity;
using SharedKernel;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.Mfa.Features.ConfirmMultiFactor;

public class ConfirmMultiFactorHandler(UserManager<UserDomain> userManager)
    : IRequestHandler<ConfirmMultiFactorCommand, Result>
{
    public async Task<Result> Handle(ConfirmMultiFactorCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.UserId);
        if (user is null)
            return Result.Failure("User not found.");

        var isValid = await userManager.VerifyTwoFactorTokenAsync(
            user,
            TokenOptions.DefaultAuthenticatorProvider,
            request.Code
        );

        if (!isValid)
            return Result.Failure("Invalid code.");

        await userManager.SetTwoFactorEnabledAsync(user, true);
        return Result.Success("2FA successfully enabled.");
    }
}