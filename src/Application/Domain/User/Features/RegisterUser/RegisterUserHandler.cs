using Domain.User;
using Microsoft.AspNetCore.Identity;
using SharedKernel;
using Thiagosza.Mediator.Core.Interfaces;
using Thiagosza.RabbitMq.Core.Interfaces;

namespace Application.Domain.User.Features.RegisterUser;

public class RegisterUserHandler(
    UserManager<UserDomain> userManager,
    IRabbitMqPublisher publisher,
    RegisterUserTelemetry telemetry
) : IRequestHandler<RegisterUserCommand, Result<RegisterUserResponse>>
{
    public async Task<Result<RegisterUserResponse>> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var user = new UserDomain
        {
            UserName = command.Email,
            Email = command.Email,
            Name = command.Name,
            CreatedAt = DateTime.Now
        };

        var result = await userManager.CreateAsync(user, command.Password);
        if (result.Succeeded)
        {
            var message = new RegisterUserMessage(user.Email, user.Name, DateTime.Now.Year.ToString());
            await publisher.PublishAsync(message, cancellationToken);
            telemetry.MarkUserRegistered(user);

            return Result.Success((RegisterUserResponse)user);
        }

        var errors = result.Errors.Select(e => e.Description).ToList();
        telemetry.MarkUserRegistrationFailed(user, errors);
        return Result.Failure<RegisterUserResponse>(errors);
    }
}
