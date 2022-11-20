using TedeeTrips.Domain.ValueObjects;

namespace TedeeTrips.Domain;

public static class Errors
{
    public static class Trip
    {
        public static Error NotFound(Guid? id = null) => new("Trip.Value.NotFound", $"Trip with id '{id}' was not found. Create it first.");
        
        public static Error NameIsNotUnique(string name) => new("Trip.Name.NotUnique", $"Trip with name '{name}' already exists. Choose another name.");

        public static Error NameHasToBeSingleLine() => new("Trip.Name.NotASingleLine", $"Trip's name cannot contain new line characters.");
    }
    
    public static class Email
    {
        public static Error InvalidValue() => new("Email.Value.Invalid", "Provided value is not a valid email address. Provide a valid email address.");
    }
    
    public static class Country
    {
        public static Error InvalidValue() => new("Country.Value.OutOfRange", "Provided value does not correspond to any known country. Provide a valid country.");
    }

    public static class Enrollment
    {
        public static Error UserAlreadyEnrolled() => new("Enrollment.EmailAddress.AlreadyEnrolled", "This email address is already enrolled for the trip. Use a different one.");

        public static Error UserNotEnrolled() => new("Enrollment.EmailAddress.NotEnrolled", "This email address is not enrolled for the trip. Use a different one.");
    }

    public static class RegisteredEmailAddress
    {
        public static Error NotRegistered() => new ("RegisteredEmailAddress.Value.NotFound", "This email address is not registered with any trip. Register it first with a trip.");
    }
}