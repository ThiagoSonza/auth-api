using Application.Infrastructure;
using Application.Infrastructure.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SharedKernel;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.Password.Features.ResetPassword;

public class ResetPasswordEndpoint : IEndPoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup(string.Empty)
            .WithTags("Credentials")
            .WithOpenApi()
            .AllowAnonymous();

        group.MapPost("reset-password", async (
            [FromServices] IMediator mediator,
            [FromBody] ResetPasswordRequest request,
            CancellationToken cancellationToken) =>
            {
                ResetPasswordCommand command = request;
                var result = await mediator.Send(command, cancellationToken);
                if (result.IsSuccess)
                    return Results.Ok(result);

                return Results.BadRequest(result);
            })
            .AddEndpointFilter<ValidationFilter<ResetPasswordRequest>>()
            .WithName("ResetPassword")
            .WithDescription("Endpoint for resetting a user's password")
            .WithSummary("Resets a user's password using a reset code")
            .Produces<Result>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);
    }
}
