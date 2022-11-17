using TedeeTrips.Domain.ValueObjects;

namespace TedeeTrips.Domain;

public static class Errors
{
    public static class Email
    {
        public static Error InvalidValue() => new Error("Email", "Provided value is not a valid email address.");
    }
}