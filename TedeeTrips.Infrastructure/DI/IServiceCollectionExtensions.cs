using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace TedeeTrips.Infrastructure.DI;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        return services
            .AddDbContext<ReservationsDbContext>(opt => opt.UseInMemoryDatabase("TedeeTrips"));
    }
}