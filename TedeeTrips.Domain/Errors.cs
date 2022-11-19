using TedeeTrips.Domain.ValueObjects;

namespace TedeeTrips.Domain;

public static class Errors
{
    public static class Trip
    {
        public static Error NotFound(Guid? id = null) => new Error("Trip.Value.NotFound", $"Trip with id '{id}' was not found. Create it first.");
        
        public static Error NameIsNotUnique(string name) => new Error("Trip.Name.NotUnique", $"Trip with name '{name}' already exists. Choose another name.");
    }
    
    public static class Email
    {
        public static Error InvalidValue() => new Error("Email.Value.Invalid", "Provided value is not a valid email address. Provide a valid email address.");
    }
    
    public static class Country
    {
        public static Error InvalidValue() => new Error("Country.Value.OutOfRange", "Provided value does not correspond to any known country. Provide a valid country.");
    }
}