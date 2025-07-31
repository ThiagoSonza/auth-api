using Application.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SharedKernel;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.Email.Features.ConfirmEmail;

public class ConfirmEmailEndpoint : IEndPoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup(string.Empty)
            .WithOpenApi()
            .WithTags("Confirm Email");

        group.MapGet("confirm-email", async (
            [FromServices] IMediator mediator,
            [FromQuery] string Token,
            [FromQuery] string Email,
            CancellationToken cancellationToken) =>
            {
                var confirmEmailCommand = ConfirmEmailCommand.Criar(Token, Email);
                var result = await mediator.Send(confirmEmailCommand, cancellationToken);
                if (result.IsSuccess)
                    return Results.Ok(result);

                return Results.BadRequest(result);
            })
            .WithName("ConfirmEmail")
            .WithDescription("Endpoint for confirming user email addresses")
            .WithSummary("Confirms a user's email address using a token")
            .Produces<Result>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .AllowAnonymous();
    }
}