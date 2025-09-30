using Worker.Infrastructure.Extensions;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddConfigurations(builder.Configuration);
builder.Services.AddInjectorConfig();
builder.Services.AddRabbitMq();
var host = builder.Build();
host.Run();
