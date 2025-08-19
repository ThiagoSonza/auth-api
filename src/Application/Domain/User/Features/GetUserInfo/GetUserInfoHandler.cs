using Domain.User;
using Microsoft.AspNetCore.Identity;
using SharedKernel;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.User.Features.GetUserInfo;

public class GetUserInfoHandler(
    UserManager<UserDomain> userManager,
    GetUserInfoTelemetry telemetry
) : IRequestHandler<GetUserInfoCommand, Result<GetUserInfoResponse>>
{
    public async Task<Result<GetUserInfoResponse>> Handle(GetUserInfoCommand command, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(command.UserId!);
        if (user is null)
        {
            telemetry.MarkUserNotFound(command.UserId);
            return Result.Failure<GetUserInfoResponse>("Usuário não encontrado.");
        }

        telemetry.MarkUserFound(user);

        GetUserInfoResponse userInfoResponse = user;
        return Result.Success(userInfoResponse);
    }
}