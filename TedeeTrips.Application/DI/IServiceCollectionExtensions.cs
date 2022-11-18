using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace TedeeTrips.Application.DI;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        return services.AddMediatR(typeof(IServiceCollectionExtensions));
    }
}