using Microsoft.AspNetCore.Identity.UI.Services;
using Thiagosza.RabbitMq.Core.Interfaces;
using Worker.Services;

namespace Worker.ManageUserContext.Password.ResetPassword;

public class ResetPasswordConsumer(
    IEmailSender emailSender,
    ResetPasswordTelemetry telemetry
) : IMessageHandler<ResetPasswordMessage>
{
    public async Task HandleAsync(ResetPasswordMessage message, CancellationToken cancellationToken)
    {
        telemetry.ReceivedMessage(message);

        var template = await new EmailTemplateRendererBuilder("ResetPassword")
            .With("Name", message.Name)
            .With("Year", DateTime.Now.Year.ToString())
            .Build();

        telemetry.SendingEmail(message);

        await emailSender.SendEmailAsync(message.Email, "Sua senha foi alterada", template);

        telemetry.EmailSent(message);
    }
}
