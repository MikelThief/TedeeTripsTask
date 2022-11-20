using System.Text.Json.Serialization;

namespace TedeeTrips.Core.Presentation;

public class Trip
{
    [JsonConstructor]
    public Trip(Guid id, string name, string country, string description, DateTimeOffset startDate, uint seatsCount)
    {
        Id = id;
        Name = name;
        Country = country;
        Description = description;
        StartDate = startDate;
        SeatsCount = seatsCount;
    }

    public static Trip From(Domain.Entities.Trip input) => 
        new Trip(input.Id, input.Name, input.Country.Name, input.Description, input.StartDate, input.SeatsCount);

    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public string Country { get; set; }
    
    public string Description { get; set; }
    
    public DateTimeOffset StartDate { get; set; }
    
    public uint SeatsCount { get; set; }
}