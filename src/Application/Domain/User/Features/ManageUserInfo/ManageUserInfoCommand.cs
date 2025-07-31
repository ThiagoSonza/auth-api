using Microsoft.AspNetCore.Identity.Data;
using SharedKernel;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.User.Features.ManageUserInfo;

public class ManageUserInfoCommand : IRequest<Result>
{
    private ManageUserInfoCommand(string userId, string? newEmail, string? newPassword, string? oldPassword)
    {
        UserId = userId;
        NewEmail = newEmail;
        NewPassword = newPassword;
        OldPassword = oldPassword;
    }

    public string UserId { get; } = string.Empty;
    public string? NewEmail { get; } = string.Empty;
    public string? NewPassword { get; } = string.Empty;
    public string? OldPassword { get; } = string.Empty;


    public static implicit operator ManageUserInfoCommand(InfoRequest request)
    {
        return new ManageUserInfoCommand(Guid.NewGuid().ToString(), request.NewEmail, request.NewPassword, request.OldPassword);
    }
}
