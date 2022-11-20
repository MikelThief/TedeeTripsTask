using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TedeeTrips.Application.Services;
using TedeeTrips.Domain;
using TedeeTrips.Domain.Commands;
using TedeeTrips.Domain.Entities;
using TedeeTrips.Domain.ValueObjects;

namespace TedeeTrips.Application.Handlers;

public class RegistrationsCommandsHandler : IRequestHandler<Register, UnitResult<ErrorArray>>, IRequestHandler<Unregister, UnitResult<ErrorArray>>
{
    private readonly IRegistrationsContext _registrationsContext;

    public RegistrationsCommandsHandler(IRegistrationsContext registrationsContext)
    {
        _registrationsContext = registrationsContext;
    }

    public async Task<UnitResult<ErrorArray>> Handle(Register request, CancellationToken cancellationToken)
    {
        return await EmailAddress
              .Create(request.EmailAddress)
              .Bind(async email =>
              {
                  Maybe<RegisteredEmailAddress> maybeRegisteredEmailAddress = (await
                      _registrationsContext.RegisteredEmailAddresses
                                           .Include(x => x.Enrollments)
                                           .ThenInclude(x => x.Trip)
                                           .Where(rea => rea.EmailAddress == email)
                                           .ToListAsync(cancellationToken)).FirstOrDefault()!;

                  await maybeRegisteredEmailAddress.ExecuteNoValue(async () =>
                  {
                      maybeRegisteredEmailAddress = new RegisteredEmailAddress(email);
                      await _registrationsContext.RegisteredEmailAddresses.AddAsync(
                          maybeRegisteredEmailAddress.GetValueOrThrow(), cancellationToken);
                  });
                  return maybeRegisteredEmailAddress.ToResult(
                      Errors.RegisteredEmailAddress.NotRegistered().ToErrorArray());
              })
              .Bind(async rea =>
              {
                  Maybe<Trip> maybeTrip = (await _registrationsContext.Trips.FindAsync(new object?[] { request.TripId }, cancellationToken))!;
                  return maybeTrip
                      .ToResult(Errors.Trip.NotFound().ToErrorArray())
                      .Map(trip => new { Trip = trip, RegisteredEmailAddress = rea });
              })
              .Check(args => args.RegisteredEmailAddress.EnrollIn(args.Trip))
              .Tap(_ => _registrationsContext.SaveChangesAsync(cancellationToken));
    }
    
    public async Task<UnitResult<ErrorArray>> Handle(Unregister request, CancellationToken cancellationToken) =>
        await EmailAddress
              .Create(request.EmailAddress)
              .Bind(async email =>
              {
                  Maybe<RegisteredEmailAddress> maybeRegisteredEmailAddress = (await
                      _registrationsContext.RegisteredEmailAddresses
                                           .Include(x => x.Enrollments)
                                           .ThenInclude(x => x.Trip)
                                           .Where(rea => rea.EmailAddress == email)
                                           .ToListAsync(cancellationToken)).FirstOrDefault()!;
                  return maybeRegisteredEmailAddress.ToResult(Errors.RegisteredEmailAddress.NotRegistered().ToErrorArray());
              })
              .Check(rea => rea.Disenroll(request.TripId))
              .Tap(_ => _registrationsContext.SaveChangesAsync(cancellationToken));
}