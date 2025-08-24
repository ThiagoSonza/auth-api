using Api.Extensions;
using Api.Extensions.CustomProblemDetails;
using Api.Extensions.Swagger;
using Api.Extensions.Telemetry;
using Thiagosza.Mediator.Core.Extensions;

var builder = WebApplication.CreateBuilder(args);
Console.WriteLine($"Current environment: {builder.Environment.EnvironmentName}");

builder.Services.AddConfigurations(builder.Configuration);
builder.Services.AddVersioning();
builder.Services.AddCors();
builder.Services.AddApiProblemDetails();
builder.Services.AdicionaSwagger();
builder.Services.AddApiAuthentication();
builder.Services.AddAuthorization();
builder.Services.RegisterServices();
builder.Services.AddMediator();
builder.Services.AddValidators();
builder.Services.AddTelemetry(builder.Host);
builder.Services.AddRabbitMq();

var app = builder.Build();
app.MapEndpoints();
app.ConfigureSwagger();
app.UseAuthentication();
app.UseAuthorization();
app.UseExceptionHandler();
app.UseStatusCodePages();
app.Run();