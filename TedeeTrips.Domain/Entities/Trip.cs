using MassTransit;
using TedeeTrips.Domain.ValueObjects;

namespace TedeeTrips.Domain.Entities;

public class Trip
{
    public NewId Id { get; set; }
    
    public string Name { get; set; }
    
    public Country Country { get; set; }
    
    public string Description { get; set; }
    
    public DateTimeOffset StartDate { get; set; }
    
    public uint SeatsCount { get; set; }
}