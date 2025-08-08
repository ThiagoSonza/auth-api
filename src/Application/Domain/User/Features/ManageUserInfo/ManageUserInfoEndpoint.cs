using Application.Infrastructure;
using Application.Infrastructure.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SharedKernel;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.User.Features.ManageUserInfo;

public class ManageUserInfoEndpoint : IEndPoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        // var group = app
        //     .MapGroup(string.Empty)
        //     .WithOpenApi()
        //     .WithTags("User Registration")
        //     .RequireAuthorization();

        // group.MapPost("manage-info", async (
        //     [FromServices] IMediator mediator,
        //     [FromBody] InfoRequest request,
        //     CancellationToken cancellationToken) =>
        //     {
        //         ManageUserInfoCommand command = request;
        //         var result = await mediator.Send(command, cancellationToken);
        //         if (result is not null)
        //             return Results.Ok(result);

        //         return Results.Problem(result);
        //     })
        //     .AddEndpointFilter<ValidationFilter<InfoRequest>>()
        //     .WithName("ManageUserInfo")
        //     .WithDescription("Endpoint to manage user information")
        //     .WithSummary("Updates the information of the authenticated user")
        //     .Produces<Result>(StatusCodes.Status200OK)
        //     .Produces(StatusCodes.Status400BadRequest)
        //     .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);
    }
}
