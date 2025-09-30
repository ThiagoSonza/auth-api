using System.Net;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SharedKernel;
using Thiagosza.RabbitMq.Core.Interfaces;
using Worker.Services;

namespace Worker.ManageUserContext.User.ConfirmEmail;

public class ResendConfirmationConsumer(
    IEmailSender emailSender,
    IOptions<AppSettings> settings,
    ResendConfirmationTelemetry telemetry
) : IMessageHandler<ResendConfirmationMessage>
{
    private readonly UrlOptions urlOptions = settings.Value.Urls;

    public async Task HandleAsync(ResendConfirmationMessage message, CancellationToken cancellationToken)
    {
        telemetry.ReceivedMessage(message);

        var url = urlOptions.UrlFrontend;

        var template = await new EmailTemplateRendererBuilder("ConfirmEmail")
            .With("Name", message.Name)
            .With("Year", DateTime.Now.Year.ToString())
            .With("ConfirmationLink", $"{url}/confirm-email?userId={message.UserId}&token={WebUtility.UrlEncode(message.Token)}")
            .Build();

        telemetry.SendingEmail(message);

        await emailSender.SendEmailAsync(message.Email, "Confirmação de E-mail", template);

        telemetry.EmailSent(message);
    }
}
