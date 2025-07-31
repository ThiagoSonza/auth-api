using System.Security.Claims;
using Application.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SharedKernel;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.Mfa.Features.ConfirmMultiFactor;

public class ConfirmMultiFactorEndpoint : IEndPoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup(string.Empty)
            .WithTags("MFA")
            .WithOpenApi()
            .RequireAuthorization();

        group.MapPost("/2fa/confirm", async (
            [FromServices] IMediator mediator,
            [FromQuery] string code,
            ClaimsPrincipal userPrincipal,
            CancellationToken cancellationToken) =>
            {
                var command = ConfirmMultiFactorCommand.Create(code, userPrincipal);
                var result = await mediator.Send(command, cancellationToken);
                if (result is not null)
                    return Results.Ok(result);

                return Results.BadRequest(result);
            })
            .WithName("ConfirmMultiFactor")
            .WithDescription("Endpoint for confirming multi-factor authentication")
            .WithSummary("Confirms a user's multi-factor authentication using a code")
            .Produces<Result>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);
    }
}