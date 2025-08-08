using Domain.User;
using Microsoft.AspNetCore.Identity;
using SharedKernel;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.User.Features.RegisterUser;

public class RegisterUserHandler(
    UserManager<UserDomain> userManager
) : IRequestHandler<RegisterUserCommand, Result<RegisterUserResponse>>
{
    public async Task<Result<RegisterUserResponse>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var usuario = new UserDomain
        {
            UserName = request.Email,
            Email = request.Email,
            PersonalIdentifier = "00112233445566"
        };

        var result = await userManager.CreateAsync(usuario, request.Password);
        if (result.Succeeded)
        {
            //     var template = await new EmailTemplateRendererBuilder("WelcomeEmail")
            //         .With("UserName", usuario.UserName!)
            //         .With("Year", DateTime.Now.Year.ToString())
            //         .Build(emailTemplateRenderer);

            //     await emailSender.SendEmailAsync(usuario.Email!, "Bem-vindo", template);
            return Result.Success((RegisterUserResponse)usuario);
        }

        var errors = result.Errors.Select(e => e.Description).ToList();
        return Result.Failure<RegisterUserResponse>(errors);
    }
}
