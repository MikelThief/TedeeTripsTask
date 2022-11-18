using MassTransit;
using TedeeTrips.Domain.Entities;
using TedeeTrips.Domain.ValueObjects;

namespace TedeeTrips.Domain.Aggregates;

public class Registration
{
    public NewId Id { get; set; }
    
    public EmailAddress EmailAddress { get; set; }
    
    public Trip Trip { get; set; }
    
    public NewId TripId { get; set; }
}