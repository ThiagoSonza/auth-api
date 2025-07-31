using Application.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SharedKernel;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.Roles.Features.GetRoleByName;

public class GetRoleByNameEndpoint : IEndPoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup(string.Empty)
            .WithOpenApi()
            .WithTags("Roles")
            .RequireAuthorization();

        group.MapGet("roles/{roleName}", async (
            [FromServices] IMediator mediator,
            [FromRoute] string roleName,
            CancellationToken cancellationToken) =>
            {
                var command = GetRoleByNameCommand.Create(roleName);
                var result = await mediator.Send(command, cancellationToken);
                if (result is not null)
                    return Results.Ok(result);

                return Results.BadRequest(result);
            })
            .WithName("GetRoleByName")
            .WithDescription("Endpoint for retrieving a role by name")
            .WithSummary("Retrieves a specific role by its name from the identity system")
            .Produces<Result<GetRoleByNameResponse>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);
    }
}
