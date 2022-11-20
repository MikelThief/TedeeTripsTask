using CSharpFunctionalExtensions;
using TedeeTrips.Domain.Commands;
using TedeeTrips.Domain.ValueObjects;

namespace TedeeTrips.Domain.Entities;

public class Trip: Entity<Guid>
{
    [Obsolete("Should not be used by anyone but EF Core and Trip class itself.")]
    protected Trip(TripName name, Country country, string description, DateTimeOffset startDate, uint seatsCount)
    {
        Name = name;
        Country = country;
        Description = description;
        StartDate = startDate;
        SeatsCount = seatsCount;
    }

    public TripName Name { get; protected set; }
    
    public Country Country { get; protected set; }
    
    public string Description { get; protected set; }
    
    public DateTimeOffset StartDate { get; protected set; }
    
    public uint SeatsCount { get; protected set; }

    public static Result<Trip, ErrorArray> Create(CreateTrip command, ICollection<TripName> takenNames)
    {
        return Country
               .FromId(command.CountryId)
               .ToResult(Errors.Country.InvalidValue().ToErrorArray())
               .Bind(country => TripName.Create(command.Name).Map(tripName => new { TripName = tripName, Country = country }))
               .Ensure(_ => takenNames.All(n => !string.Equals((string)n, command.Name, StringComparison.Ordinal)), Errors.Trip.NameIsNotUnique(command.Name))
               .Map(arg => new Trip(arg.TripName, arg.Country, command.Description, command.StartDate, command.SeatsCount));
    }
    
    public void EditInfo(TripName name, Country country, string description, DateTimeOffset startDate, uint seatsCount)
    {
        Name = name;
        Country = country;
        Description = description;
        StartDate = startDate;
        SeatsCount = seatsCount;
    }
}