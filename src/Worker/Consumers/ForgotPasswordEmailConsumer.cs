using System.Net;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Thiagosza.RabbitMq.Core.Interfaces;
using Worker.Models;
using Worker.Services;

namespace Worker.Consumers;

public class ForgotPasswordEmailConsumer(
    IEmailSender emailSender,
    IConfiguration configuration
) : IMessageHandler<ForgotPasswordEmail>
{
    public async Task HandleAsync(ForgotPasswordEmail message, CancellationToken cancellationToken)
    {
        var url = configuration.GetSection("Urls:UrlFrontend").Value;

        var template = await new EmailTemplateRendererBuilder("ForgotPassword")
            .With("UserName", message.UserName)
            .With("ResetLink", $"{url}/reset-password?userId={message.UserId}&token={WebUtility.UrlEncode(message.Token)}")
            .Build();

        await emailSender.SendEmailAsync(message.Email, "Solicitação para redefinição de senha", template);
    }
}
