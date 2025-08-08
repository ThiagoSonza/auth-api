using Application.Infrastructure;
using Application.Infrastructure.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.Password.Features.ForgotPassword;

public class ForgotPasswordEndpoint : IEndPoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("forgot-password", async (
            [FromServices] IMediator mediator,
            [FromBody] ForgotPasswordRequest request,
            CancellationToken cancellationToken) =>
            {
                ForgotPasswordCommand command = request;
                var result = await mediator.Send(command, cancellationToken);
                if (result.IsSuccess)
                    return Results.Ok(result.Value);

                return Results.Problem(result.Error?.First());
            })
            .WithTags("Credentials")
            .WithOpenApi()
            .WithName("ForgotPassword")
            .WithDescription("Endpoint for requesting a password reset")
            .WithSummary("Sends a password reset code to the user's email")
            .AddEndpointFilter<ValidationFilter<ForgotPasswordRequest>>()
            .AllowAnonymous()
            .Produces<string>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);
    }
}