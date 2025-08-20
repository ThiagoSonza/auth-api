using Api.Extensions;
using Api.Extensions.Messaging;
using Api.Extensions.Swagger;
using Api.Extensions.Telemetry;
using Thiagosza.Mediator.Core.Extensions;

var builder = WebApplication.CreateBuilder(args);
Console.WriteLine($"Current environment: {builder.Environment.EnvironmentName}");

builder.Services.AddVersioning();
builder.Services.AddCors();
builder.Services.AddApiProblemDetails();
builder.Services.AdicionaSwagger();
builder.Services.AddAuthentication(builder.Configuration);
builder.Services.AddAuthorization();
builder.Services.RegisterServices(builder.Configuration);
builder.Services.AddMediator();
builder.Services.AddValidators();
builder.Services.AddTelemetry(builder.Host, builder.Configuration);
builder.Services.AddRabbitMq(builder.Configuration);

var app = builder.Build();
app.MapEndpoints();
app.ConfigureSwagger();
app.UseAuthentication();
app.UseAuthorization();
app.Run();