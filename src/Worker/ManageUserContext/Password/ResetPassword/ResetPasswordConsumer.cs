using Microsoft.AspNetCore.Identity.UI.Services;
using Thiagosza.RabbitMq.Core.Interfaces;
using Worker.Services;

namespace Worker.ManageUserContext.Password.ResetPassword;

public class ResetPasswordConsumer(
    IEmailSender emailSender
) : IMessageHandler<ResetPasswordMessage>
{
    public async Task HandleAsync(ResetPasswordMessage message, CancellationToken cancellationToken)
    {
        var template = await new EmailTemplateRendererBuilder("ResetPassword")
            .With("UserName", message.Username)
            .Build();

        await emailSender.SendEmailAsync(message.Email, "Sua senha foi alterada", template);
    }
}
