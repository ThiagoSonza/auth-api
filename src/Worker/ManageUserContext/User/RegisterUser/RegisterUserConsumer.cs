using Microsoft.AspNetCore.Identity.UI.Services;
using Thiagosza.RabbitMq.Core.Interfaces;
using Worker.Services;

namespace Worker.ManageUserContext.User.RegisterUser;

public class RegisterUserConsumer(
    IEmailSender emailSender,
    RegisterUserTelemetry telemetry
) : IMessageHandler<RegisterUserMessage>
{
    public async Task HandleAsync(RegisterUserMessage message, CancellationToken cancellationToken)
    {
        telemetry.ReceivedMessage(message);

        var template = await new EmailTemplateRendererBuilder("WelcomeEmail")
            .With("Name", message.Name)
            .With("Year", DateTime.Now.Year.ToString())
            .Build();

        telemetry.SendingEmail(message);

        await emailSender.SendEmailAsync(message.Email, "Bem-vindo", template);

        telemetry.EmailSent(message);
    }
}