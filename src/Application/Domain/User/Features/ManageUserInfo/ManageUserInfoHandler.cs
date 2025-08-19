using Domain.User;
using Microsoft.AspNetCore.Identity;
using SharedKernel;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.User.Features.ManageUserInfo;

public class ManageUserInfoHandler(
    UserManager<UserDomain> userManager,
    ManageUserInfoTelemetry telemetry
    ) : IRequestHandler<ManageUserInfoCommand, Result>
{
    public async Task<Result> Handle(ManageUserInfoCommand command, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(command.UserId);
        if (user is null)
        {
            telemetry.MarkUserNotFound(command.UserId);
            return Result.Failure("Usuário não encontrado.");
        }

        user.Update(
            personalIdentifier: "new personal identifier",
            userName: "new username",
            email: "new email",
            emailConfirmed: user.EmailConfirmed
        );

        var result = await userManager.UpdateAsync(user);
        if (result.Succeeded)
        {
            telemetry.MarkUserUpdated(user);
            return Result.Success("Usuário atualizado com sucesso.");
        }

        var errors = result.Errors.Select(e => e.Description).ToList();
        telemetry.MarkUserNotUpdated(user, errors);
        return Result.Failure(errors);
    }
}