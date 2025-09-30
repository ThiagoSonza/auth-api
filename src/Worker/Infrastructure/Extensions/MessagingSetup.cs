using Microsoft.Extensions.Options;
using SharedKernel;
using Thiagosza.RabbitMq.Core.Extensions;
using Worker.ManageUserContext.Password.ForgotPassword;
using Worker.ManageUserContext.Password.ResetPassword;
using Worker.ManageUserContext.User.ConfirmEmail;
using Worker.ManageUserContext.User.RegisterUser;

namespace Worker.Infrastructure.Extensions;

public static class MessagingSetup
{
    public static IServiceCollection AddRabbitMq(this IServiceCollection services)
    {
        var settings = services.BuildServiceProvider().GetRequiredService<IOptions<AppSettings>>().Value.Messaging;

        services.AddRabbitMqMessaging(configure =>
        {
            configure.Host(new Uri($"amqp://{settings.Host}"), settings.Port, h =>
            {
                h.UserName = settings.Username;
                h.Password = settings.Password;
            });

            configure.AddConsumer<ForgotPasswordConsumer>(settings.Queues.ForgotPassword);
            configure.AddConsumer<ResetPasswordConsumer>(settings.Queues.ResetPassword);
            configure.AddConsumer<ResendConfirmationConsumer>(settings.Queues.ResendConfirmation);
            configure.AddConsumer<RegisterUserConsumer>(settings.Queues.RegisterUser);
        });

        return services;
    }
}
