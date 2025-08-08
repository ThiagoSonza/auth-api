using Application.Infrastructure;
using Application.Infrastructure.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.Roles.Features.CreateRole;

public class CreateRoleEndpoint : IEndPoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("roles", async (
            [FromServices] IMediator mediator,
            [FromBody] CreateRoleRequest request,
            CancellationToken cancellationToken) =>
            {
                CreateRoleCommand command = request;
                var result = await mediator.Send(command, cancellationToken);
                if (result.IsSuccess)
                    return Results.Created($"/roles/{result.Value.Id}", result.Value);

                return Results.Problem(result.Error?.First());
            })
            .WithOpenApi()
            .WithTags("Roles")
            .WithName("CreateRole")
            .WithDescription("Endpoint for creating a new role")
            .WithSummary("Creates a new role in the identity system")
            .AddEndpointFilter<ValidationFilter<CreateRoleRequest>>()
            .RequireAuthorization()
            .Produces<CreateRoleResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);
    }
}