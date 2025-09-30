using FluentValidation;

namespace Application.Domain.User.Features.RegisterUser;

public class RegisterRequestValidator : AbstractValidator<RegisterUserRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email é obrigatório.")
            .EmailAddress().WithMessage("Formato do e-mail é inválido.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome é obrigatório.")
            .MaximumLength(50).WithMessage("O nome deve ter no máximo 50 caracteres.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Senha é obrigatória.")
            .MinimumLength(8).WithMessage("A senha deve ter pelo menos 8 caracteres.")
            .Matches(@"[A-Z]").WithMessage("A senha deve conter pelo menos uma letra maiúscula.")
            .Matches(@"[a-z]").WithMessage("A senha deve conter pelo menos uma letra minúscula.")
            .Matches(@"[0-9]").WithMessage("A senha deve conter pelo menos um número.")
            .Matches(@"[\W_]").WithMessage("A senha deve conter pelo menos um caractere especial.");
    }
}
