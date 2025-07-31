using System.Reflection;
using Application;
using Application.Infrastructure;
using Asp.Versioning;
using Asp.Versioning.Builder;

namespace Api.Extensions;

public static class EndpointSetup
{
    public static void MapEndpoints(this WebApplication app)
    {
        string appName = Assembly.GetEntryAssembly()?.GetName().Name!;
        IVersionedEndpointRouteBuilder? apiVersionedBuilder = app.NewVersionedApi(appName);

        ApiVersionSet apiVersionSet = app.NewApiVersionSet()
            .HasDeprecatedApiVersion(new ApiVersion(1, 0))
            .HasApiVersion(new ApiVersion(2, 0))
            .ReportApiVersions()
            .Build();

        RouteGroupBuilder routeGroup = app
            .MapGroup("api/v{version:apiVersion}")
            .WithApiVersionSet(apiVersionSet);

        Assembly assembly = typeof(ApplicationAssembly).Assembly;

        var classes = assembly.DefinedTypes.Where(type => typeof(IEndPoint).IsAssignableFrom(type) &&
                                            type.IsAbstract == false &&
                                            type.IsInterface == false)
                                            .Select(s => Activator.CreateInstance(s) as IEndPoint);

        foreach (var endpoint in classes)
            endpoint?.MapEndpoint(routeGroup);
    }
}