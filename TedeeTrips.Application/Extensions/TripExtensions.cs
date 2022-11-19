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
            .Ensure(_ => tripNames.All(n => !string.Equals((string)n, command.Name, StringComparison.Ordinal)), Errors.Trip.NameIsNotUnique(command.Name))
            .Bind(country => TripName.Create(command.Name).Map(tripName => (tripName, country)))
            .Tap(arguments =>
            {
                trip.Country = arguments.country;
                trip.Description = command.Description;
                trip.Name = arguments.tripName;
                trip.SeatsCount = command.SeatsCount;
                trip.StartDate = command.StartDate;
            })
            .Map(_ => trip);
}