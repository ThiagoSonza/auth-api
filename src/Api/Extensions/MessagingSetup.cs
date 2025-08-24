using Application.Domain.Email.Features.ResendConfirmationEmail;
using Application.Domain.Password.Features.ForgotPassword;
using Application.Domain.Password.Features.ResetPassword;
using Application.Domain.User.Features.RegisterUser;
using Microsoft.Extensions.Options;
using SharedKernel;
using Thiagosza.RabbitMq.Core.Extensions;
using Worker.Consumers;

namespace Api.Extensions;

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

            configure.AddConsumer<ResendConfirmationEmailConsumer>(settings.Queues.ResendConfirmation);
            configure.AddConsumer<ForgotPasswordEmailConsumer>(settings.Queues.ForgotPassword);
            configure.AddConsumer<ResetPasswordEmailConsumer>(settings.Queues.ResetPassword);
            configure.AddConsumer<RegisterUserEmailConsumer>(settings.Queues.RegisterUser);

            configure.AddProducer<ResendConfirmationMessage>(settings.Queues.ResendConfirmation);
            configure.AddProducer<ForgotPasswordMessage>(settings.Queues.ForgotPassword);
            configure.AddProducer<ResetPasswordMessage>(settings.Queues.ResetPassword);
            configure.AddProducer<RegisterUserMessage>(settings.Queues.RegisterUser);
        });

        return services;
    }
}
