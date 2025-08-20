using Application.Domain.Email.Features.ResendConfirmationEmail;
using Application.Domain.Password.Features.ForgotPassword;
using Application.Domain.Password.Features.ResetPassword;
using Application.Domain.User.Features.RegisterUser;
using Thiagosza.RabbitMq.Core.Extensions;
using Worker.Consumers;

namespace Api.Extensions.Messaging;

public static class MessagingSetup
{
    public static IServiceCollection AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
    {
        var config = configuration.GetSection("Messaging").Get<RabbitConfiguration>()!;

        services.AddRabbitMqMessaging(configure =>
        {
            configure.Host(new Uri($"amqp://{config.Host}"), config.Port, h =>
            {
                h.UserName = config.Username;
                h.Password = config.Password;
            });

            configure.AddConsumer<ResendConfirmationEmailConsumer>(config.Queues.ResendConfirmation);
            configure.AddConsumer<ForgotPasswordEmailConsumer>(config.Queues.ForgotPassword);
            configure.AddConsumer<ResetPasswordEmailConsumer>(config.Queues.ResetPassword);
            configure.AddConsumer<RegisterUserEmailConsumer>(config.Queues.RegisterUser);

            configure.AddProducer<ResendConfirmationMessage>(config.Queues.ResendConfirmation);
            configure.AddProducer<ForgotPasswordMessage>(config.Queues.ForgotPassword);
            configure.AddProducer<ResetPasswordMessage>(config.Queues.ResetPassword);
            configure.AddProducer<RegisterUserMessage>(config.Queues.RegisterUser);
        });

        return services;
    }
}
