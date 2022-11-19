using Microsoft.EntityFrameworkCore;
using TedeeTrips.Application.Services;
using TedeeTrips.Domain.Aggregates;
using TedeeTrips.Domain.Entities;
using TedeeTrips.Domain.ValueObjects;

namespace TedeeTrips.Infrastructure;

public class RegistrationsDbContext : DbContext, IRegistrationsContext
{
    public RegistrationsDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Trip>()
                    .Property(t => t.Country)
                    .HasConversion(v => v.Id, v => Country.FromId(v).GetValueOrThrow("Database state does not conform to domain invariants."));

        modelBuilder.Entity<Trip>()
                    .Property(t => t.Name)
                    .HasConversion(v => v.Value, v => TripName.Create(v).Value);
        
        modelBuilder.Entity<Registration>()
                    .Property(t => t.EmailAddress)
                    .HasConversion(v => v.Value, v => EmailAddress.Create(v).Value);

        modelBuilder.Entity<Registration>()
                    .HasOne(t => t.Trip);
        
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Trip> Trips { get; set; }
    
    public DbSet<Registration> Registrations { get; set; }
}