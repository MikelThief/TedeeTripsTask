using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TedeeTrips.Application.Query;
using TedeeTrips.Application.Services;
using TedeeTrips.Domain.Entities;
using TedeeTrips.Domain.ValueObjects;

namespace TedeeTrips.Application.Handlers;

// for the sake of simplicity we put all trips related queries in one place
public class TripQueriesHandler : IRequestHandler<GetTrip, Maybe<Trip>>, IRequestHandler<GetTripsByCountry, ICollection<Trip>>, IRequestHandler<GetTrips, ICollection<Trip>>
{
    private readonly IRegistrationsContext _registrationsContext;
    
    public TripQueriesHandler(IRegistrationsContext registrationsContext)
    {
        _registrationsContext = registrationsContext;
    }
    
    public async Task<Maybe<Trip>> Handle(GetTrip request, CancellationToken cancellationToken) =>
        Maybe.From(await _registrationsContext.Trips.FindAsync(new object?[] { request.Id }, cancellationToken))!;

    public async Task<ICollection<Trip>> Handle(GetTripsByCountry request, CancellationToken cancellationToken) =>
        // unless the path is hot, we can force creation of async state machine with async-await usage to have cleaner stacktrace
        await _registrationsContext.Trips.Where(t => t.Country == Country.FromName(request.Country)).ToListAsync(cancellationToken);

    public async Task<ICollection<Trip>> Handle(GetTrips request, CancellationToken cancellationToken) =>
        // unless the path is hot, we can force creation of async state machine with async-await usage to have cleaner stacktrace
        await _registrationsContext.Trips.ToListAsync(cancellationToken);
}