using Domain.User;
using Microsoft.AspNetCore.Identity;
using SharedKernel;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.Email.Features.ConfirmEmail;

public class ConfirmEmailHandler(UserManager<UserDomain> userManager) : IRequestHandler<ConfirmEmailCommand, Result>
{
    public async Task<Result> Handle(ConfirmEmailCommand command, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(command.Email);
        if (user == null)
            return Result.Failure("Usuário não encontrado.");

        var result = await userManager.ConfirmEmailAsync(user, command.Token);
        if (result.Succeeded)
            return Result.Success("E-mail confirmado com sucesso.");

        var errors = result.Errors.Select(e => e.Description).ToList();
        return Result.Failure(errors);
    }
}
