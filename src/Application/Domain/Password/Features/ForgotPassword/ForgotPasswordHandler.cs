using Domain.User;
using Microsoft.AspNetCore.Identity;
using SharedKernel;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.Password.Features.ForgotPassword;

public class ForgotPasswordHandler(
    UserManager<UserDomain> userManager
) : IRequestHandler<ForgotPasswordCommand, Result<string>>
{
    public async Task<Result<string>> Handle(ForgotPasswordCommand command, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(command.Email);
        if (user is null)
            return Result.Failure<string>("Usuário não encontrado");

        if (!await userManager.IsEmailConfirmedAsync(user))
            return Result.Failure<string>("Essa conta precisa confirmar seu e-mail antes de realizar o login");

        var token = await userManager.GeneratePasswordResetTokenAsync(user);

        // var template = await new EmailTemplateRendererBuilder("ForgotPassword")
        //     .With("UserName", user.UserName!)
        //     .With("ResetLink", $"http://localhost:5000/reset-password?userId={user.Id}&token={WebUtility.UrlEncode(token)}")
        //     .Build(emailTemplateRenderer);

        // await emailSender.SendEmailAsync(user.Email!, "Solicitação para redefinição de senha", template);

        return Result.Success("Email de redefinição de senha enviado com sucesso");
    }
}
