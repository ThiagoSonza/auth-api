using System.Security.Claims;
using Application.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.Mfa.Features.EnableMultiFactor;

public class EnableMultiFactorEndpoint : IEndPoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/enable", async (
            [FromServices] IMediator mediator,
            ClaimsPrincipal userPrincipal,
            CancellationToken cancellationToken) =>
            {
                var command = EnableMultiFactorCommand.Create(userPrincipal);
                var result = await mediator.Send(command, cancellationToken);
                if (result.IsSuccess)
                    return Results.Ok(result.Value);

                return Results.Problem(result.Error?.First());
            })
            .WithOpenApi()
            .WithTags("MFA")
            .WithName("EnableMultiFactor")
            .WithDescription("Endpoint for enabling multi-factor authentication")
            .WithSummary("Enables a user's multi-factor authentication")
            .RequireAuthorization()
            .Produces<string>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);
    }
}
