using Worker.Infrastructure.Extensions;
using Worker.Infrastructure.Extensions.Telemetry;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddConfigurations(builder.Configuration);
builder.Services.AddTelemetry(builder);
builder.Services.AddInjectorConfig();
builder.Services.AddRabbitMq();

var app = builder.Build();
await app.RunAsync();
