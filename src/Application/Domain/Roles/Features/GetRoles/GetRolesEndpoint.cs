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
        app.MapGet("roles", async (
            [FromServices] IMediator mediator,
            CancellationToken cancellationToken) =>
            {
                var command = GetRolesCommand.Create();
                var result = await mediator.Send(command, cancellationToken);
                if (result.IsSuccess && result.Value.Any())
                    return Results.Ok(result.Value);

                return Results.NotFound("No roles found.");
            })
            .WithOpenApi()
            .WithTags("Roles")
            .WithName("GetRoles")
            .WithDescription("Endpoint for retrieving all roles")
            .WithSummary("Retrieves a list of all roles in the identity system")
            .RequireAuthorization()
            .Produces<IEnumerable<IdentityRole>>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);
    }
}
