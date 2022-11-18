using Microsoft.Extensions.DependencyInjection;

namespace TedeeTrips.Infrastructure.DI;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddDomainLayer(this IServiceCollection services)
    {
        return services;
    }
}