using System.Security.Claims;
using Application.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SharedKernel;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.User.Features.GetUserInfo;

public class GetUserInfoEndpoint : IEndPoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup(string.Empty)
            .WithOpenApi()
            .WithTags("User Registration")
            .RequireAuthorization();

        group.MapGet("info", async (
            [FromServices] IMediator mediator,
            CancellationToken cancellationToken,
            HttpContext httpContext) =>
            {
                GetUserInfoCommand command = (httpContext.User.Identity as ClaimsIdentity)!;
                var result = await mediator.Send(command, cancellationToken);
                if (result is not null)
                    return Results.Ok(result);

                return Results.BadRequest(result);
            })
            .WithName("GetUserInfo")
            .WithDescription("Endpoint to retrieve user information")
            .WithSummary("Fetches the information of the authenticated user")
            .Produces<Result<GetUserInfoResponse>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);
    }
}
