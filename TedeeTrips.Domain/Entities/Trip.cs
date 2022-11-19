using CSharpFunctionalExtensions;
using MassTransit;
using TedeeTrips.Domain.Commands;
using TedeeTrips.Domain.ValueObjects;

namespace TedeeTrips.Domain.Entities;

public class Trip
{
    [Obsolete("Should not be used by anyone but EF Core")]
    protected Trip(Guid id, TripName name, Country country, string description, DateTimeOffset startDate, uint seatsCount)
    {
        Id = id;
        Name = name;
        Country = country;
        Description = description;
        StartDate = startDate;
        SeatsCount = seatsCount;
    }

    public Guid Id { get; set; }
    
    public TripName Name { get; set; }
    
    public Country Country { get; set; }
    
    public string Description { get; set; }
    
    public DateTimeOffset StartDate { get; set; }
    
    public uint SeatsCount { get; set; }
    
    public static Trip Reconsitute(Guid id, TripName name, Country country, string description, DateTimeOffset startDate, uint seatsCount)
    {
        return new Trip(id, name, country, description, startDate, seatsCount);
    }

    public static Result<Trip, ErrorArray> Create(CreateTrip command, ICollection<TripName> takenNames)
    {
        return Country
               .FromId(command.CountryId)
               .ToResult(Errors.Country.InvalidValue().ToErrorArray())
               .Bind(country => TripName.Create(command.Name).Map(tripName => (tripName, country)))
               .Ensure(_ => takenNames.All(n => !string.Equals((string)n, command.Name, StringComparison.Ordinal)), Errors.Trip.NameIsNotUnique(command.Name))
               .Map(arguments => Reconsitute(NewId.Next().ToSequentialGuid(), arguments.tripName, arguments.country, command.Description, command.StartDate, command.SeatsCount));
    }
}