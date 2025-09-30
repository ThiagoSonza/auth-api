using Domain.User;
using Microsoft.AspNetCore.Identity;
using SharedKernel;
using Thiagosza.Mediator.Core.Interfaces;
using Thiagosza.RabbitMq.Core.Interfaces;

namespace Application.Domain.Password.Features.ForgotPassword;

public class ForgotPasswordHandler(
    UserManager<UserDomain> userManager,
    IRabbitMqPublisher publisher,
    ForgotPasswordTelemetry telemetry
) : IRequestHandler<ForgotPasswordCommand, Result<string>>
{
    public async Task<Result<string>> Handle(ForgotPasswordCommand command, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(command.Email);
        if (user is null)
        {
            telemetry.MarkUserNotFound(command.Email);
            return Result.Failure<string>("Usuário não encontrado");
        }

        if (!await userManager.IsEmailConfirmedAsync(user))
        {
            telemetry.MarkEmailNotConfirmed(user);
            return Result.Failure<string>("Essa conta precisa confirmar seu e-mail antes de realizar o login");
        }

        var token = await userManager.GeneratePasswordResetTokenAsync(user);

        var message = new ForgotPasswordMessage(user.Id, user.Name!, user.Email!, token);
        await publisher.PublishAsync(message, cancellationToken);
        telemetry.MarkEmailSent(user);

        return Result.Success("Email de redefinição de senha enviado com sucesso");
    }
}
