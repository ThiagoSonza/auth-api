using Domain.User;
using Microsoft.AspNetCore.Identity;
using SharedKernel;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.Mfa.Features.GetStatus;

public class GetStatusHandler(
    UserManager<UserDomain> userManager
) : IRequestHandler<GetStatusCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(GetStatusCommand command, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(command.UserId);
        if (user == null)
            return Result.Failure<bool>("Usuário não encontrado.");

        var is2faEnabled = await userManager.GetTwoFactorEnabledAsync(user);

        return Result.Success(is2faEnabled);
    }
}
