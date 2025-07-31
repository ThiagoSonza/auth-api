using Application.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.Roles.Features.GetRoles;

public class GetRolesEndpoint : IEndPoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup(string.Empty)
            .WithOpenApi()
            .WithTags("Roles")
            .RequireAuthorization();

        group.MapGet("roles", async (
            [FromServices] IMediator mediator,
            CancellationToken cancellationToken) =>
            {
                var command = GetRolesCommand.Create();
                var result = await mediator.Send(command, cancellationToken);
                if (result is not null)
                    return Results.Ok(result);

                return Results.NotFound("No roles found.");
            })
            .WithName("GetRoles")
            .WithDescription("Endpoint for retrieving all roles")
            .WithSummary("Retrieves a list of all roles in the identity system")
            .Produces<IEnumerable<IdentityRole>>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);
    }
}
