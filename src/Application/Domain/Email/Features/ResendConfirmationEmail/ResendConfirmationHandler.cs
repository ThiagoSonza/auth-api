using Domain.User;
using Microsoft.AspNetCore.Identity;
using SharedKernel;
using Thiagosza.Mediator.Core.Interfaces;
using Thiagosza.RabbitMq.Core.Interfaces;

namespace Application.Domain.Email.Features.ResendConfirmationEmail;

public class ResendConfirmationHandler(
    UserManager<UserDomain> userManager,
    IRabbitMqPublisher publisher,
    ResendConfirmationTelemetry telemetry
    ) : IRequestHandler<ResendConfirmationEmailCommand, Result<string>>
{
    public async Task<Result<string>> Handle(ResendConfirmationEmailCommand command, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(command.Email);
        if (user is null)
        {
            telemetry.MarkUserNotFound(command.Email);
            return Result.Failure<string>("Usuário não encontrado.");
        }

        if (await userManager.IsEmailConfirmedAsync(user))
        {
            telemetry.MarkEmailAlreadyConfirmed(user);
            return Result.Failure<string>("Esse e-mail já foi confirmado.");
        }

        var message = new ResendConfirmationMessage(
            user.Id,
            user.Name!,
            user.Email!,
            await userManager.GenerateEmailConfirmationTokenAsync(user));

        await publisher.PublishAsync(message, cancellationToken);
        telemetry.MarkEmailResent(user);

        return Result.Success("Email de confirmação enviado com sucesso.");
    }
}