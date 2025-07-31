using Application;
using Application.Infrastructure;

namespace Api.Extensions;

public static class EndpointSetup
{
    public static void MapEndpoints(this WebApplication app)
    {
        // custom endpoints
        var routeGroup = app.MapGroup("/api");
        var assembly = typeof(ApplicationAssembly).Assembly;

        var classes = assembly.DefinedTypes.Where(type => typeof(IEndPoint).IsAssignableFrom(type) &&
                                            type.IsAbstract == false &&
                                            type.IsInterface == false)
                                            .Select(s => Activator.CreateInstance(s) as IEndPoint);

        foreach (var endpoint in classes)
            endpoint?.MapEndpoint(routeGroup);
    }
}