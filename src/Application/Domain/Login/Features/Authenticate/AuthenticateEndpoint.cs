using Application.Infrastructure;
using Application.Infrastructure.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.Login.Features.Authenticate;

public class AuthenticateEndpoint : IEndPoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("login", async (
            [FromServices] IMediator mediator,
            [FromBody] LoginRequest request,
            CancellationToken cancellationToken) =>
            {
                AuthenticateCommand command = request;
                var result = await mediator.Send(command, cancellationToken);
                if (result.IsSuccess)
                    return Results.Ok(result.Value);

                return Results.Problem(result.Error?.First());
            })
            .WithOpenApi()
            .WithTags("Authentication")
            .WithName("Login")
            .WithDescription("Endpoint for user login")
            .WithSummary("Authenticates a user and returns a JWT token")
            .AllowAnonymous()
            .AddEndpointFilter<ValidationFilter<LoginRequest>>()
            .Produces<AuthenticateResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);
    }
}
