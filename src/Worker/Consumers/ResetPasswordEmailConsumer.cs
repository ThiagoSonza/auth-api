using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Thiagosza.RabbitMq.Core.Interfaces;
using Worker.Models;
using Worker.Services;

namespace Worker.Consumers;

public class ResetPasswordEmailConsumer(
    IEmailSender emailSender,
    IConfiguration configuration
) : IMessageHandler<ResetPasswordEmail>
{
    public async Task HandleAsync(ResetPasswordEmail message, CancellationToken cancellationToken)
    {
        var url = configuration.GetSection("Urls:UrlFrontend").Value;

        var template = await new EmailTemplateRendererBuilder("ResetPassword")
            .With("UserName", message.Username)
            .Build();

        await emailSender.SendEmailAsync(message.Email, "Sua senha foi alterada", template);
    }
}
