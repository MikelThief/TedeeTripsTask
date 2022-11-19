using CSharpFunctionalExtensions;

namespace TedeeTrips.Domain.ValueObjects;

public class TripName : SimpleValueObject<string>
{
    private TripName(string value) : base(value)
    {
    }
    
    public static Result<TripName, ErrorArray> Create(string value)
    {
        if (value.Contains('\r') || value.Contains('\n'))
        {
            return Errors.Trip.NameHasToBeSingleLine().ToErrorArray();
        }

        return new TripName(value);
    }
}