using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;

namespace Services;

public class EmailSender(IConfiguration configuration) : IEmailSender
{
    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        try
        {
            var client = GetDefaultClient();
            var senderEmail = configuration.GetValue<string>("Email:Sender");

            var mailMessage = new MailMessage
            {
                From = new MailAddress(senderEmail!),
                IsBodyHtml = true,
                Subject = subject,
                Body = htmlMessage,
                To = { new MailAddress(email) }
            };

            await client.SendMailAsync(mailMessage);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private SmtpClient GetDefaultClient()
    {
        var client = new SmtpClient(configuration.GetValue<string>("Email:Host"), configuration.GetValue<int>("Email:Port"))
        {
            Credentials = new NetworkCredential(configuration.GetValue<string>("Email:User"), configuration.GetValue<string>("Email:Password")),
            EnableSsl = false
        };

        return client;
    }
}