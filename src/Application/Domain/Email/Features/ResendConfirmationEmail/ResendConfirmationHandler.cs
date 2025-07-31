using Domain.User;
using Microsoft.AspNetCore.Identity;
using SharedKernel;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.Email.Features.ResendConfirmationEmail;

public class ResendConfirmationHandler(UserManager<UserDomain> userManager) : IRequestHandler<ResendConfirmationEmailCommand, Result>
{
    public async Task<Result> Handle(ResendConfirmationEmailCommand command, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(command.Email);
        if (user == null)
            return Result.Failure("Usuário não encontrado.");

        if (await userManager.IsEmailConfirmedAsync(user))
            return Result.Failure("Esse e-mail já foi confirmado.");

        // var template = await new EmailTemplateRendererBuilder("ConfirmEmail")
        //     .With("UserName", user.UserName!)
        //     .With("ConfirmationLink", $"http://localhost:5000/confirm-email?userId={user.Id}&token={WebUtility.UrlEncode(await userManager.GenerateEmailConfirmationTokenAsync(user))}")
        //     .Build(emailTemplateRenderer);

        // await emailSender.SendEmailAsync(user.Email!, "Confirmação de E-mail", template);

        return Result.Success("Email de confirmação enviado com sucesso.");
    }
}