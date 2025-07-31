using FluentValidation;
using Microsoft.AspNetCore.Identity.Data;

namespace Application.Domain.Password.Features.ForgotPassword;

public class ForgotPasswordValidator : AbstractValidator<ForgotPasswordRequest>
{
    public ForgotPasswordValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email é obrigatório.")
            .EmailAddress().WithMessage("Formato do e-mail é inválido.");
    }
}