using Domain.User;
using Microsoft.AspNetCore.Identity;
using SharedKernel;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.Mfa.Features.EnableMultiFactor;

public class EnableMultiFactorHandler(
    UserManager<UserDomain> userManager
) : IRequestHandler<EnableMultiFactorCommand, Result<string>>
{
    public async Task<Result<string>> Handle(EnableMultiFactorCommand command, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(command.UserId);
        if (user is null)
            return Result.Failure<string>("Usuário não encontrado");

        await userManager.SetTwoFactorEnabledAsync(user, true);
        return Result.Success("2FA ativado com sucesso");
    }
}