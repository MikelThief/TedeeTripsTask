using CSharpFunctionalExtensions;
using MediatR;
using TedeeTrips.Domain.Entities;
using TedeeTrips.Domain.ValueObjects;

namespace TedeeTrips.Domain.Commands;

// Commands with many members remain classes to maintain OpenApi model discovery
public class CreateTrip : IRequest<Result<Trip, ErrorArray>>
{
    public string Name { get; init; }
    
    public int CountryId { get; init; }
    
    public string Description { get; init; }
    
    public DateTimeOffset StartDate { get; init; }
    
    public uint SeatsCount { get; init; }
}