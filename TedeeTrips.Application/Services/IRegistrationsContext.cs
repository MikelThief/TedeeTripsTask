using Microsoft.EntityFrameworkCore;
using TedeeTrips.Domain.Aggregates;
using TedeeTrips.Domain.Entities;

namespace TedeeTrips.Application.Services;

public interface IRegistrationsContext
{
    public DbSet<Trip> Trips { get; set; }
    
    public DbSet<Registration> Registrations { get; set; }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}