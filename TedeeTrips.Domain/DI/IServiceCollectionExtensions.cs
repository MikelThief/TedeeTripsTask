﻿using Microsoft.Extensions.DependencyInjection;

namespace TedeeTrips.Infrastructure.DI;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        return services;
    }
}