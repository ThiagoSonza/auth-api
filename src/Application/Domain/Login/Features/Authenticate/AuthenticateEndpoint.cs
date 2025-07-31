using Application.Infrastructure;
using Application.Infrastructure.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SharedKernel;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.Login.Features.Authenticate;

public class AuthenticateEndpoint : IEndPoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup(string.Empty)
            .WithOpenApi()
            .WithTags("Authentication")
            .AllowAnonymous();

        group.MapPost("login", async (
            [FromServices] IMediator mediator,
            [FromBody] LoginRequest request,
            CancellationToken cancellationToken) =>
            {
                AuthenticateCommand command = request;
                var result = await mediator.Send(command, cancellationToken);
                if (result.IsSuccess)
                    return Results.Ok(result);

                return Results.Unauthorized();
            })
            .AddEndpointFilter<ValidationFilter<LoginRequest>>()
            .WithName("Login")
            .WithDescription("Endpoint for user login")
            .WithSummary("Authenticates a user and returns a JWT token")
            .Produces<Result<AuthenticateResponse>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);
    }
}
