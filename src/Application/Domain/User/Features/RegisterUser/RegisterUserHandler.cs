using Domain.User;
using Microsoft.AspNetCore.Identity;
using SharedKernel;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.User.Features.RegisterUser;

public class RegisterUserHandler(
    UserManager<UserDomain> userManager,
    RegisterUserTelemetry telemetry
) : IRequestHandler<RegisterUserCommand, Result<RegisterUserResponse>>
{
    public async Task<Result<RegisterUserResponse>> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var user = new UserDomain
        {
            UserName = command.Email,
            Email = command.Email,
            PersonalIdentifier = "00112233445566"
        };

        var result = await userManager.CreateAsync(user, command.Password);
        if (result.Succeeded)
        {
            //     var template = await new EmailTemplateRendererBuilder("WelcomeEmail")
            //         .With("UserName", usuario.UserName!)
            //         .With("Year", DateTime.Now.Year.ToString())
            //         .Build(emailTemplateRenderer);

            //     await emailSender.SendEmailAsync(usuario.Email!, "Bem-vindo", template);

            telemetry.MarkUserRegistered(user);

            return Result.Success((RegisterUserResponse)user);
        }

        var errors = result.Errors.Select(e => e.Description).ToList();
        telemetry.MarkUserRegistrationFailed(user, errors);
        return Result.Failure<RegisterUserResponse>(errors);
    }
}
