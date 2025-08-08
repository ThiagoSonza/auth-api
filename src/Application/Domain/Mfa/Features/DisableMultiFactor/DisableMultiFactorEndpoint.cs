using System.Security.Claims;
using Application.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.Mfa.Features.DisableMultiFactor;

public class DisableMultiFactorEndpoint : IEndPoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/disable", async (
            [FromServices] IMediator mediator,
            ClaimsPrincipal userPrincipal,
            CancellationToken cancellationToken) =>
            {
                var command = DisableMultiFactorCommand.Create(userPrincipal);
                var result = await mediator.Send(command, cancellationToken);
                if (result.IsSuccess)
                    return Results.Ok(result.Value);

                return Results.Problem(result.Error?.First());
            })
            .WithOpenApi()
            .WithTags("MFA")
            .WithName("DisableMultiFactor")
            .WithDescription("Endpoint for disable multi-factor authentication")
            .WithSummary("Disable a user's multi-factor authentication")
            .RequireAuthorization()
            .Produces<string>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);
    }
}
