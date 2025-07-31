namespace Application.Domain.Mfa.Features.GenerateQrCode;

public record GenerateQrCodeResponse
{
    private GenerateQrCodeResponse(string key, string qrCodeImageBase64, string manualEntryKey)
    {
        Key = key;
        QrCodeImageBase64 = qrCodeImageBase64;
        ManualEntryKey = manualEntryKey;
    }

    public string Key { get; init; }
    public string QrCodeImageBase64 { get; init; }
    public string ManualEntryKey { get; init; }

    public static GenerateQrCodeResponse Create(string key, string qrCodeImageBase64, string manualEntryKey)
    {
        return new GenerateQrCodeResponse(key, qrCodeImageBase64, manualEntryKey);
    }
}
