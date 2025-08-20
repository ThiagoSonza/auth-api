using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Thiagosza.RabbitMq.Core.Interfaces;
using Worker.Models;
using Worker.Services;

namespace Worker.Consumers;

public class RegisterUserEmailConsumer(
    IEmailSender emailSender,
    IConfiguration configuration
) : IMessageHandler<RegisterUserEmail>
{
    public async Task HandleAsync(RegisterUserEmail message, CancellationToken cancellationToken)
    {
        var url = configuration.GetSection("Urls:UrlFrontend").Value;

        var template = await new EmailTemplateRendererBuilder("WelcomeEmail")
            .With("UserName", message.Username)
            .With("Year", DateTime.Now.Year.ToString())
            .Build();

        await emailSender.SendEmailAsync(message.Username, "Bem-vindo", template);
    }
}