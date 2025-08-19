using Domain.User;
using Microsoft.AspNetCore.Identity;
using SharedKernel;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.Password.Features.ResetPassword;

public class ResetPasswordHandler(
    UserManager<UserDomain> userManager,
    ResetPasswordTelemetry telemetry
) : IRequestHandler<ResetPasswordCommand, Result<string>>
{
    public async Task<Result<string>> Handle(ResetPasswordCommand command, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(command.Email);
        if (user is null)
        {
            telemetry.MarkUserNotFound(command.Email);
            return Result.Failure<string>("Usuário não encontrado");
        }

        if (!await userManager.IsEmailConfirmedAsync(user))
        {
            telemetry.MarkEmailNotConfirmed(user);
            return Result.Failure<string>("Essa conta precisa confirmar seu e-mail antes de realizar o login");
        }

        var result = await userManager.ResetPasswordAsync(user, command.ResetCode, command.NewPassword);
        if (result.Succeeded)
        {
            //     var template = await new EmailTemplateRendererBuilder("ResetPassword")
            //         .With("UserName", user.UserName!)
            //         .Build(emailTemplateRenderer);

            // await emailSender.SendEmailAsync(user.Email!, "Sua senha foi alterada", template);

            telemetry.MarkPasswordChanged(user);
            return Result.Success("Senha alterada com sucesso");
        }

        var errors = result.Errors.Select(e => e.Description).ToList();
        return Result.Failure<string>(errors);
    }
}