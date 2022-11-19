using CSharpFunctionalExtensions;

namespace TedeeTrips.Domain.Entities;

public class Enrollment : Entity<Guid>
{
    public RegisteredEmailAddress RegisteredEmailAddress { get; set; }
    
    public Trip Trip { get; set; }
}