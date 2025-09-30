using Application.Infrastructure;
using Application.Infrastructure.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.User.Features.RegisterUser;

public class RegisterUserEndpoint : IEndPoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("register", async (
            [FromServices] IMediator mediator,
            [FromBody] RegisterUserRequest request,
            CancellationToken cancellationToken) =>
            {
                RegisterUserCommand command = request;
                var result = await mediator.Send(command, cancellationToken);
                if (result.IsSuccess)
                    return Results.Created($"/user/{result.Value.Id}", result.Value);

                return Results.Problem(result.Error?.First());
            })
            .WithOpenApi()
            .WithTags("User Registration")
            .WithName("RegisterUser")
            .WithDescription("Endpoint to register a new user")
            .WithSummary("Registers a new user in the system")
            .AddEndpointFilter<ValidationFilter<RegisterRequest>>()
            .Produces<RegisterUserResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);
    }
}
