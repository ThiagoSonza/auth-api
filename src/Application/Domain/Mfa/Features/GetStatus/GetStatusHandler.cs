using Domain.User;
using Microsoft.AspNetCore.Identity;
using SharedKernel;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.Mfa.Features.GetStatus;

public class GetStatusHandler(
    UserManager<UserDomain> userManager
) : IRequestHandler<GetStatusCommand, Result>
{
    public async Task<Result> Handle(GetStatusCommand command, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(command.UserId);
        if (user == null)
            return Result.Failure("Usuário não encontrado.");

        var is2faEnabled = await userManager.GetTwoFactorEnabledAsync(user);

        return is2faEnabled
            ? Result.Success("2FA is enabled.")
            : Result.Failure("2FA is disabled.");
    }
}
