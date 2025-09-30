using Microsoft.AspNetCore.Identity.UI.Services;
using Thiagosza.RabbitMq.Core.Interfaces;
using Worker.Models;
using Worker.Services;

namespace Worker.ManageUserContext.User.RegisterUser;

public class RegisterUserConsumer(
    IEmailSender emailSender
) : IMessageHandler<RegisterUserMessage>
{
    public async Task HandleAsync(RegisterUserMessage message, CancellationToken cancellationToken)
    {
        var template = await new EmailTemplateRendererBuilder("WelcomeEmail")
            .With("Name", message.Name)
            .With("Year", DateTime.Now.Year.ToString())
            .Build();

        await emailSender.SendEmailAsync(message.Username, "Bem-vindo", template);
    }
}