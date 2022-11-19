using CSharpFunctionalExtensions;
using MassTransit;
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
        var emailAddressResult = EmailAddress.Create(request.EmailAddress);

        if (emailAddressResult.IsFailure)
        {
            return emailAddressResult;
        }

        Maybe<RegisteredEmailAddress> maybeRegisteredEmailAddress = (await
                _registrationsContext.RegisteredEmailAddresses
                                     .Include(x => x.Enrollments)
                                     .ThenInclude(x => x.Trip)
                                     .ToListAsync(cancellationToken))
                                     .FirstOrDefault(rea => rea.EmailAddress == emailAddressResult.Value)!;

      
        await maybeRegisteredEmailAddress.ExecuteNoValue(
            async () =>
            {
                maybeRegisteredEmailAddress = new RegisteredEmailAddress()
                    {EmailAddress = emailAddressResult.Value};
                await _registrationsContext.RegisteredEmailAddresses.AddAsync(
                    maybeRegisteredEmailAddress.GetValueOrThrow(), cancellationToken);
            });

        var maybeTrip = Maybe.From(await _registrationsContext.Trips.FindAsync(new object?[] { request.TripId }, cancellationToken));

        if (maybeTrip.HasValue)
        {
            var enrollResult = maybeRegisteredEmailAddress.GetValueOrThrow().EnrollIn(maybeTrip.GetValueOrThrow()!);
            
            if (enrollResult.IsFailure)
            {
                return UnitResult.Failure(enrollResult.Error);
            }
            else
            {
                await _registrationsContext.SaveChangesAsync(cancellationToken);
                return UnitResult.Success<ErrorArray>();
            }
        }

        return Errors.Trip.NotFound().ToErrorArray();
    }


    public async Task<UnitResult<ErrorArray>> Handle(Unregister request, CancellationToken cancellationToken)
    {
        var emailAddressResult = EmailAddress.Create(request.EmailAddress);

        if (emailAddressResult.IsFailure)
        {
            return emailAddressResult;
        }

        Maybe<RegisteredEmailAddress> maybeRegisteredEmailAddress = (await
            _registrationsContext.RegisteredEmailAddresses
                                 .Include(x => x.Enrollments)
                                 .ThenInclude(x => x.Trip)
                                 .Where(rea => rea.EmailAddress == emailAddressResult.Value)
                                 .ToListAsync(cancellationToken)).FirstOrDefault()!;

        if (maybeRegisteredEmailAddress.HasNoValue)
        {
            return UnitResult.Failure(Errors.RegisteredEmailAddress.NotRegistered().ToErrorArray());
        }

        var disenrollResult = maybeRegisteredEmailAddress.GetValueOrThrow().Disenroll(request.TripId);

        if (disenrollResult.IsFailure)
        {
            return disenrollResult;
        }
        
        await _registrationsContext.SaveChangesAsync(cancellationToken);
        return UnitResult.Success<ErrorArray>();
    }
}