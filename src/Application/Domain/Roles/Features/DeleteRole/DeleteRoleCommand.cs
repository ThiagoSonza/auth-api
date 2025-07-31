using SharedKernel;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.Roles.Features.DeleteRole
{
    public record DeleteRoleCommand : IRequest<Result>
    {
        private DeleteRoleCommand(string roleName)
        {
            RoleName = roleName;
        }

        public string RoleName { get; }

        public static DeleteRoleCommand Create(string roleName)
        {
            return new DeleteRoleCommand(roleName);
        }
    }
}