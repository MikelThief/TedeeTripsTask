using CSharpFunctionalExtensions;
using TedeeTrips.Domain.ValueObjects;

namespace TedeeTrips.Domain.Entities;

public class RegisteredEmailAddress : Entity<Guid>
{
    public RegisteredEmailAddress(EmailAddress emailAddress)
    {
        EmailAddress = emailAddress;
        _enrollments = new List<Enrollment>();
    }

    public EmailAddress EmailAddress { get; protected set; }
    
    private readonly List<Enrollment> _enrollments;
    public virtual IReadOnlyList<Enrollment> Enrollments => _enrollments.ToList();

    public UnitResult<ErrorArray> EnrollIn(Trip trip)
    {
        return UnitResult.FailureIf(Enrollments.Any(x => x.Trip == trip), Errors.Enrollment.UserAlreadyEnrolled().ToErrorArray())
                         .Tap(() =>
                         {
                             var enrollment = new Enrollment { Trip = trip, RegisteredEmailAddress = this };
                             _enrollments.Add(enrollment);
                         });
    }
    
    public UnitResult<ErrorArray> Disenroll(Guid tripId)
    {
        return Maybe
            .From(_enrollments.FirstOrDefault(x => x.Trip.Id == tripId)!)
            .ToResult(Errors.Enrollment.UserNotEnrolled().ToErrorArray())
            .Tap(e => _enrollments.Remove(e));
    }
}