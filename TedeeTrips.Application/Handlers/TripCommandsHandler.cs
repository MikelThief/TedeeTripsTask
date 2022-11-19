using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TedeeTrips.Application.Extensions;
using TedeeTrips.Application.Services;
using TedeeTrips.Domain;
using TedeeTrips.Domain.Commands;
using TedeeTrips.Domain.Entities;
using TedeeTrips.Domain.ValueObjects;

namespace TedeeTrips.Application.Handlers;

// for the sake of simplicity we put all trips related commands in one place
public class TripCommandsHandler : IRequestHandler<CreateTrip, Result<Trip, ErrorArray>>, IRequestHandler<DeleteTrip>, IRequestHandler<ModifyTrip, UnitResult<ErrorArray>>
{
    private readonly IRegistrationsContext _registrationsContext;

    public TripCommandsHandler(IRegistrationsContext registrationsContext)
    {
        _registrationsContext = registrationsContext;
    }
    
    public async Task<Result<Trip, ErrorArray>> Handle(CreateTrip request, CancellationToken cancellationToken)
    {
        var tripNames = await _registrationsContext.Trips.Select(t => t.Name).ToListAsync(cancellationToken);
        
        return await Trip.Create(request, tripNames)
                   .Tap(t => _registrationsContext.Trips.AddAsync(t, cancellationToken))
                   .Tap(_ => _registrationsContext.SaveChangesAsync(cancellationToken));
    }

    public async Task<Unit> Handle(DeleteTrip request, CancellationToken cancellationToken)
    {
        var maybeTrip = await _registrationsContext.Trips.FindAsync(new object?[] {request.Id}, cancellationToken);
        
        if (maybeTrip is not null)
        {
            _registrationsContext.Trips.Remove(maybeTrip);
            await _registrationsContext.SaveChangesAsync(cancellationToken);
        }
        return Unit.Value;
    }

    public async Task<UnitResult<ErrorArray>> Handle(ModifyTrip request, CancellationToken cancellationToken)
    {
        var maybeTrip = await _registrationsContext.Trips.FindAsync(new object?[] {request.Id}, cancellationToken);

        if (maybeTrip is null)
        {
            return UnitResult.Failure(Errors.Trip.NotFound(request.Id).ToErrorArray());
        }
        
        var tripNames = await _registrationsContext.Trips.Select(t => t.Name).ToListAsync(cancellationToken);
        tripNames.RemoveAt(tripNames.FindIndex(x => string.Equals((string)x, maybeTrip.Name, StringComparison.Ordinal)));
        
        return maybeTrip.UpdateFrom(request, tripNames);
    }
}