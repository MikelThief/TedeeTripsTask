using CSharpFunctionalExtensions;
using TedeeTrips.Domain;
using TedeeTrips.Domain.Commands;
using TedeeTrips.Domain.Entities;
using TedeeTrips.Domain.ValueObjects;

namespace TedeeTrips.Application.Extensions;

public static class TripExtensions
{
    public static Result<Trip, ErrorArray> UpdateFrom(this Trip trip, ModifyTrip command, ICollection<TripName> tripNames) =>
        Country
            .FromId(command.CountryId)
            .ToResult(Errors.Country.InvalidValue().ToErrorArray())
            // Alternatively, in real-word complex scenario,
            // the call to Ensure below could be replaced with a domain service
            // that receives TripNames and new trip name
            .Ensure(_ => tripNames.All(n => !string.Equals((string) n, command.Name, StringComparison.Ordinal)),
                    Errors.Trip.NameIsNotUnique(command.Name))
            .Bind(country => TripName.Create(command.Name).Map(tripName => new { Tripname = tripName, Country = country }))
            .Tap(arguments => trip.EditInfo(arguments.Tripname, arguments.Country, command.Description,
                                            command.StartDate, command.SeatsCount))
            .Map(_ => trip);
}