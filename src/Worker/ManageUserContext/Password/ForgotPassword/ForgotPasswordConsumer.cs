using System.Net;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SharedKernel;
using Thiagosza.RabbitMq.Core.Interfaces;
using Worker.Services;

namespace Worker.ManageUserContext.Password.ForgotPassword;

public class ForgotPasswordConsumer(
    IEmailSender emailSender,
    IOptions<AppSettings> settings
) : IMessageHandler<ForgotPasswordMessage>
{
    private readonly UrlOptions urlOptions = settings.Value.Urls;

    public async Task HandleAsync(ForgotPasswordMessage message, CancellationToken cancellationToken)
    {
        var url = urlOptions.UrlFrontend;

        var template = await new EmailTemplateRendererBuilder("ForgotPassword")
            .With("UserName", message.UserName)
            .With("ResetLink", $"{url}/reset-password?userId={message.UserId}&token={WebUtility.UrlEncode(message.Token)}")
            .Build();

        await emailSender.SendEmailAsync(message.Email, "Solicitação para redefinição de senha", template);
    }
}
