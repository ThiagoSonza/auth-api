using Domain.User;
using Microsoft.AspNetCore.Identity;
using SharedKernel;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.Mfa.Features.EnableMultiFactor;

public class EnableMultiFactorHandler(
    UserManager<UserDomain> userManager
) : IRequestHandler<EnableMultiFactorCommand, Result>
{
    public async Task<Result> Handle(EnableMultiFactorCommand command, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(command.UserId);
        if (user is null)
            return Result.Failure("Usuário não encontrado");

        var token = await userManager.GenerateTwoFactorTokenAsync(user, TokenOptions.DefaultAuthenticatorProvider);
        var isValid = await userManager.VerifyTwoFactorTokenAsync(user, TokenOptions.DefaultAuthenticatorProvider, token);

        if (!isValid)
            return Result.Failure("Token inválido");

        await userManager.SetTwoFactorEnabledAsync(user, true);
        return Result.Success("2FA ativado com sucesso");
    }
}