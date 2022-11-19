using CSharpFunctionalExtensions;
using MassTransit;
using TedeeTrips.Domain.ValueObjects;

namespace TedeeTrips.Domain.Entities;

public class RegisteredEmailAddress : Entity<Guid>
{
    public EmailAddress EmailAddress { get; set; }
    
    private readonly List<Enrollment> _enrollments = new();
    public virtual IReadOnlyList<Enrollment> Enrollments => _enrollments.ToList();

    public UnitResult<ErrorArray> EnrollIn(Trip trip)
    {
        if (Enrollments.Any(x => x.Trip == trip))
        {
            return UnitResult.Failure<ErrorArray>(Errors.Enrollment.UserAlreadyEnrolled());
        }

        var enrollment = new Enrollment
        {
            Trip = trip,
            RegisteredEmailAddress = this
        };
        
        _enrollments.Add(enrollment);

        return UnitResult.Success<ErrorArray>();
    }
    
    public UnitResult<ErrorArray> Disenroll(Guid tripId)
    {
        var maybeEnrollment = Maybe.From(_enrollments.FirstOrDefault(x => x.Trip.Id == tripId));

        if (maybeEnrollment.HasNoValue)
        {
            return UnitResult.Failure<ErrorArray>(Errors.Enrollment.UserNotEnrolled());
        }

        _enrollments.Remove(maybeEnrollment.GetValueOrThrow()!);
        return UnitResult.Success<ErrorArray>();
    }
}