using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TedeeTrips.Application.Services;
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
        modelBuilder.Entity<RegisteredEmailAddress>(e =>
        {
            var converter = new ValueConverter<EmailAddress, string>(
                from => from.Value,
                to => EmailAddress.Create(to).Value);
            
            e.Property(r => r.EmailAddress)
             .HasConversion(converter);

            e
                .HasMany(r => r.Enrollments)
                .WithOne(r => r.RegisteredEmailAddress)
                .OnDelete(DeleteBehavior.Cascade)
                .Metadata.PrincipalToDependent!.SetPropertyAccessMode(PropertyAccessMode.Field);
        });
        
        modelBuilder.Entity<Trip>(e =>
        {
            e.Property(t => t.Country)
             .HasConversion(
                 v => v.Id,
                 v => Country.FromId(v).GetValueOrThrow("Database state does not conform to domain invariants."));
            
            e.Property(t => t.Name)
                .HasConversion(v => v.Value, v => TripName.Create(v).Value);
        });

        modelBuilder.Entity<Enrollment>(e =>
        {
            e.HasOne(p => p.RegisteredEmailAddress).WithMany(p => p.Enrollments);
            e.HasOne(p => p.Trip).WithMany();
        });
        
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Trip> Trips { get; set; }
    public DbSet<RegisteredEmailAddress> RegisteredEmailAddresses { get; set; }
}