using System.Security.Claims;
using Application.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SharedKernel;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.Login.Features.RefreshToken;

public class RefreshTokenEndpoint : IEndPoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup(string.Empty)
            .WithOpenApi()
            .WithTags("Authentication");

        group.MapPost("refresh-token", async (
            [FromServices] IMediator mediator,
            HttpContext httpContext,
            CancellationToken cancellationToken) =>
            {
                RefreshTokenCommand identity = (httpContext.User.Identity as ClaimsIdentity)!;
                var result = await mediator.Send(identity, cancellationToken);
                if (result.IsSuccess)
                    return Results.Ok(result);

                return Results.Unauthorized();
            })
            .WithName("RefreshToken")
            .WithDescription("Endpoint for refreshing JWT token")
            .WithSummary("Refreshes a JWT token for an authenticated user")
            .Produces<Result<RefreshTokenResponse>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .RequireAuthorization();
    }
}
