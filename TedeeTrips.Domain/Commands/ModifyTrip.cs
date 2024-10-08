﻿using CSharpFunctionalExtensions;
using MediatR;
using TedeeTrips.Domain.ValueObjects;

namespace TedeeTrips.Domain.Commands;

public class ModifyTrip :IRequest<UnitResult<ErrorArray>>
{
    public Guid Id { get; init; }
    
    public string Name { get; init; }
    
    public int CountryId { get; init; }
    
    public string Description { get; init; }
    
    public DateTimeOffset StartDate { get; init; }
    
    public uint SeatsCount { get; init; }
}