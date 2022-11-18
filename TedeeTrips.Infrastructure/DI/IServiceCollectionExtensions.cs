using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using TedeeTrips.Application.Services;

namespace TedeeTrips.Infrastructure.DI;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services)
    {
        return services
            .AddDbContext<RegistrationsDbContext>(opt => opt.UseInMemoryDatabase("TedeeTrips"))
            .AddScoped<IRegistrationsContext, RegistrationsDbContext>();
    }
}