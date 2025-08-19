using System.Web;
using Domain.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using QRCoder;
using SharedKernel;
using Thiagosza.Mediator.Core.Interfaces;

namespace Application.Domain.Mfa.Features.GenerateQrCode;

public class GenerateQrCodeHandler(
    UserManager<UserDomain> userManager,
    IConfiguration configuration,
    GenerateQrCodeTelemetry telemetry
) : IRequestHandler<GenerateQrCodeCommand, Result<GenerateQrCodeResponse>>
{
    public async Task<Result<GenerateQrCodeResponse>> Handle(GenerateQrCodeCommand command, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(command.UserId);
        if (user is null)
        {
            telemetry.MarkUserNotFound(command.UserId);
            return Result.Failure<GenerateQrCodeResponse>("Usuário não encontrado.");
        }

        var key = (await userManager.GetAuthenticatorKeyAsync(user))!;
        if (string.IsNullOrEmpty(key))
        {
            await userManager.ResetAuthenticatorKeyAsync(user);
            key = (await userManager.GetAuthenticatorKeyAsync(user))!;
        }

        var email = user.Email ?? "usuario";
        var appName = configuration["2FA:AppName"] ?? "MinhaApp";

        var uri = $"otpauth://totp/{HttpUtility.UrlEncode(appName)}:{HttpUtility.UrlEncode(email)}?secret={key}&issuer={HttpUtility.UrlEncode(appName)}&digits=6";

        // Gerar QR Code em base64
        var qrGenerator = new QRCodeGenerator();
        var qrCodeData = qrGenerator.CreateQrCode(uri, QRCodeGenerator.ECCLevel.Q);
        var qrCode = new Base64QRCode(qrCodeData);
        var qrCodeImageAsBase64 = qrCode.GetGraphic(20);
        var image = $"data:image/png;base64,{qrCodeImageAsBase64}";

        var response = GenerateQrCodeResponse.Create(key, image, key);
        telemetry.MarkQrCodeGenerated(command.UserId, image, key);
        return Result.Success(response);
    }
}