using FluentValidation;
using Microsoft.AspNetCore.Identity.Data;

namespace Application.Domain.User.Features.ManageUserInfo;

public class ManageIndoValidator : AbstractValidator<InfoRequest>
{
    public ManageIndoValidator()
    {
        RuleFor(x => x.NewEmail)
            .NotEmpty().WithMessage("Email é obrigatório.")
            .EmailAddress().WithMessage("Formato do e-mail é inválido.");

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("Senha é obrigatória.")
            .MinimumLength(8).WithMessage("A senha deve ter pelo menos 6 caracteres.")
            .Matches(@"[A-Z]").WithMessage("A senha deve conter pelo menos uma letra maiúscula.")
            .Matches(@"[a-z]").WithMessage("A senha deve conter pelo menos uma letra minúscula.")
            .Matches(@"[0-9]").WithMessage("A senha deve conter pelo menos um número.")
            .Matches(@"[\W_]").WithMessage("A senha deve conter pelo menos um caractere especial.");

        RuleFor(x => x.OldPassword)
            .NotEmpty().WithMessage("Senha é obrigatória.")
            .MinimumLength(8).WithMessage("A senha deve ter pelo menos 6 caracteres.")
            .Matches(@"[A-Z]").WithMessage("A senha deve conter pelo menos uma letra maiúscula.")
            .Matches(@"[a-z]").WithMessage("A senha deve conter pelo menos uma letra minúscula.")
            .Matches(@"[0-9]").WithMessage("A senha deve conter pelo menos um número.")
            .Matches(@"[\W_]").WithMessage("A senha deve conter pelo menos um caractere especial.");
    }
}
