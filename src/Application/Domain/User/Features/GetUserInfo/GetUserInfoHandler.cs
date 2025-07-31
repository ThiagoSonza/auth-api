using Domain.User;
using Microsoft.AspNetCore.Identity;
using SharedKernel;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.User.Features.GetUserInfo;

public class GetUserInfoHandler(
    UserManager<UserDomain> userManager
) : IRequestHandler<GetUserInfoCommand, Result<GetUserInfoResponse>>
{
    public async Task<Result<GetUserInfoResponse>> Handle(GetUserInfoCommand command, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(command.UserId!);
        if (user is null)
            return Result.Failure<GetUserInfoResponse>("Usuário não encontrado.");

        GetUserInfoResponse userInfoResponse = user;
        return Result.Success(userInfoResponse);
    }
}