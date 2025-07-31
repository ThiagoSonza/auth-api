using Application.Infrastructure;
using Application.Infrastructure.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SharedKernel;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.User.Features.RegisterUser;

public class RegisterUserEndpoint : IEndPoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup(string.Empty)
            .WithOpenApi()
            .WithTags("User Registration")
            .RequireAuthorization();

        app.MapPost("register", async (
            [FromServices] IMediator mediator,
            [FromBody] RegisterRequest request,
            CancellationToken cancellationToken) =>
            {
                RegisterUserCommand command = request;
                var result = await mediator.Send(command, cancellationToken);
                if (result is not null)
                    return Results.Ok(result);

                return Results.BadRequest(result);
            })
            .AddEndpointFilter<ValidationFilter<RegisterRequest>>()
            .WithOpenApi()
            .WithTags("User Registration")
            .WithName("RegisterUser")
            .WithDescription("Endpoint to register a new user")
            .WithSummary("Registers a new user in the system")
            .Produces<Result>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);
    }
}
