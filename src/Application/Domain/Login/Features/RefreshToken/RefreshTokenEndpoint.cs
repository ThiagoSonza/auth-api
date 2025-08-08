using System.Security.Claims;
using Application.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.Login.Features.RefreshToken;

public class RefreshTokenEndpoint : IEndPoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("refresh-token", async (
            [FromServices] IMediator mediator,
            HttpContext httpContext,
            CancellationToken cancellationToken) =>
            {
                RefreshTokenCommand identity = (httpContext.User.Identity as ClaimsIdentity)!;
                var result = await mediator.Send(identity, cancellationToken);
                if (result.IsSuccess)
                    return Results.Ok(result.Value);

                return Results.Problem(result.Error?.First());
            })
            .WithOpenApi()
            .WithTags("Authentication")
            .WithName("RefreshToken")
            .WithDescription("Endpoint for refreshing JWT token")
            .WithSummary("Refreshes a JWT token for an authenticated user")
            .RequireAuthorization()
            .Produces<RefreshTokenResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);
    }
}
