using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TedeeTrips.Application.Services;
using TedeeTrips.Domain.Commands;
using TedeeTrips.Domain.Entities;
using TedeeTrips.Domain.ValueObjects;

namespace TedeeTrips.Application.Handlers;

public class CreateTripCommandHandler : IRequestHandler<CreateTrip, Result<Trip, ErrorArray>>
{
    private readonly IRegistrationsContext _registrationsContext;

    public CreateTripCommandHandler(IRegistrationsContext registrationsContext)
    {
        _registrationsContext = registrationsContext;
    }
    
    public Task<Result<Trip, ErrorArray>> Handle(CreateTrip request, CancellationToken cancellationToken)
    {
        return Task.FromResult(Trip.Create(request))
                        .Tap(t => _registrationsContext.Trips.AddAsync(t, cancellationToken))
                        .Tap(_ => _registrationsContext.SaveChangesAsync(cancellationToken));
    }
}