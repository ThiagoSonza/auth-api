using Application.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.Email.Features.ConfirmEmail;

public class ConfirmEmailEndpoint : IEndPoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("confirm-email", async (
            [FromServices] IMediator mediator,
            [FromQuery] string Token,
            [FromQuery] string Email,
            CancellationToken cancellationToken) =>
            {
                var confirmEmailCommand = ConfirmEmailCommand.Criar(Token, Email);
                var result = await mediator.Send(confirmEmailCommand, cancellationToken);
                if (result.IsSuccess)
                    return Results.Ok(result.Value);

                return Results.Problem(result.Error?.First());
            })
            .WithOpenApi()
            .WithTags("Confirm Email")
            .WithName("ConfirmEmail")
            .WithDescription("Endpoint for confirming user email addresses")
            .WithSummary("Confirms a user's email address using a token")
            .AllowAnonymous()
            .Produces<string>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);
    }
}