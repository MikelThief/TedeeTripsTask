using CSharpFunctionalExtensions;
using MediatR;
using TedeeTrips.Domain.ValueObjects;

namespace TedeeTrips.Domain.Commands;

public class Unregister : IRequest<UnitResult<ErrorArray>>
{
    public Guid TripId { get; init; }
    
    public string EmailAddress { get; init; }
}