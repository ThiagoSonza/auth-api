using Microsoft.AspNetCore.Routing;

namespace Application.Infrastructure;

public interface IEndPoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}
