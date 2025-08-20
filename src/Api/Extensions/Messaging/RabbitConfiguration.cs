namespace Api.Extensions.Messaging;

public record RabbitConfiguration(string Host, int Port, string Username, string Password, RabbitConfigurationQueues Queues);

public record RabbitConfigurationQueues(string ResendConfirmation,
    string RegisterUser,
    string ForgotPassword,
    string ResetPassword);