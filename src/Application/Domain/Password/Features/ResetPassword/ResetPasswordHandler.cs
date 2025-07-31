using Domain.User;
using Microsoft.AspNetCore.Identity;
using SharedKernel;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.Password.Features.ResetPassword;

public class ResetPasswordHandler(
    UserManager<UserDomain> userManager
) : IRequestHandler<ResetPasswordCommand, Result>
{
    public async Task<Result> Handle(ResetPasswordCommand command, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(command.Email);

        if (user is null)
            return Result.Failure("Usuário não encontrado");

        if (!await userManager.IsEmailConfirmedAsync(user))
            return Result.Failure("Essa conta precisa confirmar seu e-mail antes de realizar o login");

        var result = await userManager.ResetPasswordAsync(user, command.Token, command.NewPassword);
        // if (result.Succeeded)
        // {
        //     var template = await new EmailTemplateRendererBuilder("ResetPassword")
        //         .With("UserName", user.UserName!)
        //         .Build(emailTemplateRenderer);

        //     await emailSender.SendEmailAsync(user.Email!, "Sua senha foi alterada", template);
        // }

        var errors = result.Errors.Select(e => e.Description).ToList();
        return Result.Failure(errors);
    }
}