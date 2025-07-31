using System.Security.Claims;
using Application.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SharedKernel;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.Mfa.Features.GetStatus;

public class GetStatusEndpoint : IEndPoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup(string.Empty)
            .WithTags("MFA")
            .WithOpenApi()
            .RequireAuthorization();

        group.MapGet("/status", async (
            [FromServices] IMediator mediator,
            ClaimsPrincipal userPrincipal,
            CancellationToken cancellationToken) =>
            {
                var command = GetStatusCommand.Create(userPrincipal);
                var result = await mediator.Send(command, cancellationToken);
                if (result is not null)
                    return Results.Ok(result);

                return Results.BadRequest(result);
            })
            .WithName("GetMfaStatus")
            .WithDescription("Endpoint for retrieving multi-factor authentication status")
            .WithSummary("Retrieves the multi-factor authentication status for a user")
            .Produces<Result>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);
    }
}
