using Application.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SharedKernel;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.Roles.Features.DeleteRole;

public class DeleteRoleEndpoint : IEndPoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup(string.Empty)
            .WithOpenApi()
            .WithTags("Roles")
            .RequireAuthorization();

        group.MapDelete("roles/{roleName}", async (
            [FromServices] IMediator mediator,
            [FromRoute] string roleName,
            CancellationToken cancellationToken) =>
            {
                var command = DeleteRoleCommand.Create(roleName);
                var result = await mediator.Send(command, cancellationToken);
                if (result is not null)
                    return Results.Ok(result);

                return Results.BadRequest(result);
            })
            .WithName("DeleteRole")
            .WithDescription("Endpoint for deleting a role")
            .WithSummary("Deletes a specified role from the identity system")
            .Produces<Result>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);
    }
}
