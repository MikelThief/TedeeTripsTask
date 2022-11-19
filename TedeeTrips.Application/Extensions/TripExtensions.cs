using CSharpFunctionalExtensions;
using TedeeTrips.Domain;
using TedeeTrips.Domain.Commands;
using TedeeTrips.Domain.Entities;
using TedeeTrips.Domain.ValueObjects;

namespace TedeeTrips.Application.Extensions;

public static class TripExtensions
{
    public static Result<Trip, ErrorArray> UpdateFrom(this Trip trip, ModifyTrip command, ICollection<string> tripNames) =>
        Country
            .FromId(command.CountryId)
            .ToResult(Errors.Country.InvalidValue().ToErrorArray())
            .Ensure(_ => tripNames.All(n => !string.Equals(n, command.Name, StringComparison.Ordinal)), Errors.Trip.NameIsNotUnique(command.Name))
            .Tap(c =>
            {
                trip.Country = c;
                trip.Description = command.Description;
                trip.Name = command.Name;
                trip.SeatsCount = command.SeatsCount;
                trip.StartDate = command.StartDate;
            })
            .Map(_ => trip);
}