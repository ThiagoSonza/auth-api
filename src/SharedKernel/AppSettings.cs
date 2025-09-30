namespace SharedKernel;

public record AppSettings
{
    public MessagingOptions Messaging { get; init; } = default!;
    public UrlOptions Urls { get; init; } = default!;
    public EmailOptions Email { get; init; } = default!;
    public ConnectionStringsOptions ConnectionStrings { get; init; } = default!;
    public JwtOptions Jwt { get; init; } = default!;
    public TelemetryOptions Telemetry { get; init; } = default!;
}

public record UrlOptions(string UrlFrontend);
public record MessagingOptions(string Host, int Port, string Username, string Password, RabbitConfigurationQueues Queues);
public record EmailOptions(string Host, string Port, string Username, string Password);
public record ConnectionStringsOptions(string DefaultConnection);
public record JwtOptions(string Secret, string Issuer, string Audience, int ExpiryMinutes);
public record TelemetryOptions(string MinimumLevel, string Endpoint);
public record RabbitConfigurationQueues(string ResendConfirmation, string RegisterUser, string ForgotPassword, string ResetPassword);