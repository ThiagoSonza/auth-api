using System.Security.Claims;
using Application.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.Mfa.Features.GenerateQrCode;

public class GenerateQrCodeEndpoint : IEndPoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/setup", async (
            [FromServices] IMediator mediator,
            ClaimsPrincipal userPrincipal,
            CancellationToken cancellationToken) =>
            {
                var command = GenerateQrCodeCommand.Create(userPrincipal);
                var result = await mediator.Send(command, cancellationToken);
                if (result.IsSuccess)
                    return Results.Ok(result.Value);

                return Results.Problem(result.Error?.First());
            })
            .WithOpenApi()
            .WithTags("MFA")
            .WithName("GenerateQrCode")
            .WithDescription("Endpoint for generating a QR code for multi-factor authentication setup")
            .WithSummary("Generates a QR code for setting up multi-factor authentication")
            .RequireAuthorization()
            .Produces<GenerateQrCodeResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);
    }
}
