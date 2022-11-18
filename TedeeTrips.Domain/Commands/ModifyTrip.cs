namespace TedeeTrips.Domain.Commands;

public class ModifyTrip : CreateTrip
{
    public Guid Id { get; }
}