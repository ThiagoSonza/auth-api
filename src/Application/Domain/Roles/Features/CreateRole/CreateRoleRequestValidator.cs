using FluentValidation;

namespace Application.Domain.Roles.Features.CreateRole;

public class CreateRoleRequestValidator : AbstractValidator<CreateRoleRequest>
{
    public CreateRoleRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Nome é obrigatório.")
            .MinimumLength(6).WithMessage("O nome deve ter pelo menos 6 caracteres.")
            .MaximumLength(50).WithMessage("O nome não pode exceder 50 caracteres.");
    }
}