using System.Security.Claims;
using Application.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.Mfa.Features.ConfirmMultiFactor;

public class ConfirmMultiFactorEndpoint : IEndPoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/2fa/confirm", async (
            [FromServices] IMediator mediator,
            [FromQuery] string code,
            ClaimsPrincipal userPrincipal,
            CancellationToken cancellationToken) =>
            {
                var command = ConfirmMultiFactorCommand.Create(code, userPrincipal);
                var result = await mediator.Send(command, cancellationToken);
                if (result.IsSuccess)
                    return Results.Ok(result.Value);

                return Results.Problem(result.Error?.First());
            })
            .WithOpenApi()
            .WithTags("MFA")
            .WithName("ConfirmMultiFactor")
            .WithDescription("Endpoint for confirming multi-factor authentication")
            .WithSummary("Confirms a user's multi-factor authentication using a code")
            .RequireAuthorization()
            .Produces<string>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);
    }
}