namespace TedeeTrips.Core.Presentation;

public class DescriptionlessTrip
{
    private DescriptionlessTrip(Guid id, string name, string country, DateTimeOffset startDate, uint seatsCount)
    {
        Id = id;
        Name = name;
        Country = country;
        StartDate = startDate;
        SeatsCount = seatsCount;
    }

    public static DescriptionlessTrip From(Domain.Entities.Trip input) => 
        new DescriptionlessTrip(input.Id, input.Name, input.Country.Name, input.StartDate, input.SeatsCount);

    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public string Country { get; set; }
    
    public DateTimeOffset StartDate { get; set; }
    
    public uint SeatsCount { get; set; } 
}