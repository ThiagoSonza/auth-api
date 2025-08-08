using System.Security.Claims;
using Application.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.User.Features.GetUserInfo;

public class GetUserInfoEndpoint : IEndPoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("info", async (
            [FromServices] IMediator mediator,
            CancellationToken cancellationToken,
            HttpContext httpContext) =>
            {
                GetUserInfoCommand command = (httpContext.User.Identity as ClaimsIdentity)!;
                var result = await mediator.Send(command, cancellationToken);
                if (result.IsSuccess)
                    return Results.Ok(result.Value);

                return Results.Problem(result.Error?.First());
            })
            .WithOpenApi()
            .WithTags("User Registration")
            .WithName("GetUserInfo")
            .WithDescription("Endpoint to retrieve user information")
            .WithSummary("Fetches the information of the authenticated user")
            .RequireAuthorization()
            .Produces<GetUserInfoResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);
    }
}
