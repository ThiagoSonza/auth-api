using Application.Infrastructure;
using Application.Infrastructure.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SharedKernel;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.Email.Features.ResendConfirmationEmail;

public class ResendConfirmationEmailEndpoint : IEndPoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup(string.Empty)
            .WithOpenApi()
            .WithTags("Confirm Email");

        group.MapPost("resend-confirm-email", async (
            [FromServices] IMediator mediator,
            [FromBody] ResendConfirmationEmailRequest request,
            CancellationToken cancellationToken) =>
            {
                ResendConfirmationEmailCommand command = request;
                var result = await mediator.Send(command, cancellationToken);
                if (result.IsSuccess)
                    return Results.Ok(result);

                return Results.BadRequest(result);
            })
            .AddEndpointFilter<ValidationFilter<ResendConfirmationEmailRequest>>()
            .WithName("ResendConfirmationEmail")
            .WithDescription("Endpoint for resending confirmation email")
            .WithSummary("Resends a confirmation email to the user")
            .Produces<Result>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .AllowAnonymous();
    }
}
