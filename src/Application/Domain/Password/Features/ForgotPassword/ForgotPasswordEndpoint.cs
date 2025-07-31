using Application.Infrastructure;
using Application.Infrastructure.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SharedKernel;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.Password.Features.ForgotPassword;

public class ForgotPasswordEndpoint : IEndPoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup(string.Empty)
            .WithTags("Credentials")
            .WithOpenApi()
            .AllowAnonymous();

        group.MapPost("forgot-password", async (
            [FromServices] IMediator mediator,
            [FromBody] ForgotPasswordRequest request,
            CancellationToken cancellationToken) =>
            {
                ForgotPasswordCommand command = request;
                var result = await mediator.Send(command, cancellationToken);
                if (result.IsSuccess)
                    return Results.Ok(result);

                return Results.BadRequest(result);
            })
            .AddEndpointFilter<ValidationFilter<ForgotPasswordRequest>>()
            .WithName("ForgotPassword")
            .WithDescription("Endpoint for requesting a password reset")
            .WithSummary("Sends a password reset code to the user's email")
            .Produces<Result>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);
    }
}