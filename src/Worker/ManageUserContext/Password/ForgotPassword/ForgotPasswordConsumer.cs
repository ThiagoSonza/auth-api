using System.Net;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SharedKernel;
using Thiagosza.RabbitMq.Core.Interfaces;
using Worker.Services;

namespace Worker.ManageUserContext.Password.ForgotPassword;

public class ForgotPasswordConsumer(
    IEmailSender emailSender,
    IOptions<AppSettings> settings,
    ForgotPasswordTelemetry telemetry
) : IMessageHandler<ForgotPasswordMessage>
{
    private readonly UrlOptions urlOptions = settings.Value.Urls;

    public async Task HandleAsync(ForgotPasswordMessage message, CancellationToken cancellationToken)
    {
        telemetry.ReceivedMessage(message);

        var url = urlOptions.UrlFrontend;

        var template = await new EmailTemplateRendererBuilder("ForgotPassword")
            .With("Name", message.Name)
            .With("Year", DateTime.Now.Year.ToString())
            .With("ResetLink", $"{url}/reset-password?userId={message.UserId}&token={WebUtility.UrlEncode(message.Token)}")
            .Build();

        telemetry.SendingEmail(message);

        await emailSender.SendEmailAsync(message.Email, "Solicitação para redefinição de senha", template);

        telemetry.EmailSent(message);
    }
}
