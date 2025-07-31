using FluentValidation;
using Microsoft.AspNetCore.Identity.Data;

namespace Application.Domain.Email.Features.ResendConfirmationEmail;

public class ResendConfirmationEmailValidator : AbstractValidator<ResendConfirmationEmailRequest>
{
    public ResendConfirmationEmailValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email é obrigatório.")
            .EmailAddress().WithMessage("Formato do e-mail é inválido.");
    }
}
