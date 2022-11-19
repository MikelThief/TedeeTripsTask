namespace TedeeTrips.Core.Presentation;

public class ModifyTripRequest
{
    public string Name { get; init; }
    
    public int CountryId { get; init; }
    
    public string Description { get; init; }
    
    public DateTimeOffset StartDate { get; init; }
    
    public uint SeatsCount { get; init; }
}