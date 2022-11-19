using Microsoft.EntityFrameworkCore;
using TedeeTrips.Domain.Entities;

namespace TedeeTrips.Application.Services;

public interface IRegistrationsContext
{
    public DbSet<Trip> Trips { get; set; }
    
    public DbSet<RegisteredEmailAddress> RegisteredEmailAddresses { get; set; }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}