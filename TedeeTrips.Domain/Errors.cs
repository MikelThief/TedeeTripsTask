using TedeeTrips.Domain.ValueObjects;

namespace TedeeTrips.Domain;

public static class Errors
{
    public static class Email
    {
        public static Error InvalidValue() => new Error("Email.Value.Invalid", "Provided value is not a valid email address.");
    }
    
    public static class Country
    {
        public static Error InvalidValue() => new Error("Country.Value.OutOfRange", "Provided value does not correspond to any known country.");
    }
}