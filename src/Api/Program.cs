using Api.Extensions;
using Thiagosza.Mediator.Core.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors();
builder.Services.AddApiProblemDetails();
builder.Services.AdicionaSwagger();
builder.Services.AddAuthentication(builder.Configuration);
builder.Services.AddAuthorization();
builder.Services.RegisterServices(builder.Configuration);
builder.Services.AddMediator();
builder.Services.AddValidators();

var app = builder.Build();
app.MapEndpoints();
app.ConfigureSwagger();
app.UseAuthentication();
app.UseAuthorization();
app.Run();