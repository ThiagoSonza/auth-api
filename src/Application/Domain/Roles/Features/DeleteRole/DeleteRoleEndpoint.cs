using Application.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.Roles.Features.DeleteRole;

public class DeleteRoleEndpoint : IEndPoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("roles/{roleId}", async (
            [FromServices] IMediator mediator,
            [FromRoute] string roleId,
            CancellationToken cancellationToken) =>
            {
                var command = DeleteRoleCommand.Create(roleId);
                var result = await mediator.Send(command, cancellationToken);
                if (result.IsSuccess)
                    return Results.Ok(result.Value);

                return Results.Problem(result.Error?.First());
            })
            .WithOpenApi()
            .WithTags("Roles")
            .WithName("DeleteRole")
            .WithDescription("Endpoint for deleting a role")
            .WithSummary("Deletes a specified role from the identity system")
            .RequireAuthorization()
            .Produces<string>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);
    }
}
