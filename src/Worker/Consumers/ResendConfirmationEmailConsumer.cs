using System.Net;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Thiagosza.RabbitMq.Core.Interfaces;
using Worker.Models;
using Worker.Services;

namespace Worker.Consumers;

public class ResendConfirmationEmailConsumer(
    IEmailSender emailSender,
    IConfiguration configuration
) : IMessageHandler<ResendConfirmationEmail>
{
    public async Task HandleAsync(ResendConfirmationEmail message, CancellationToken cancellationToken)
    {
        var url = configuration.GetSection("Urls:UrlFrontend").Value;

        var template = await new EmailTemplateRendererBuilder("ConfirmEmail")
            .With("UserName", message.Username)
            .With("ConfirmationLink", $"{url}/confirm-email?userId={message.UserId}&token={WebUtility.UrlEncode(message.Token)}")
            .Build();

        await emailSender.SendEmailAsync(message.Email, "Confirmação de E-mail", template);
    }
}
