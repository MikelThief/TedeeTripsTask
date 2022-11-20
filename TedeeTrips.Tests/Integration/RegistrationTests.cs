using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TedeeTrips.Core.Controllers;
using TedeeTrips.Core.Presentation;
using TedeeTrips.Domain.Commands;
using TedeeTrips.Tests.Fixtures;
using Xunit;

namespace TedeeTrips.Tests.Integration;

[Collection(nameof(ApiCollection))]
public class RegistrationTests : IClassFixture<TestHostFixture>
{
    private readonly TestHostFixture _fixture;

    public RegistrationTests(TestHostFixture fixture)
    {
        _fixture = fixture;
    }

    public async Task Register_once_and_deregister_once_for_trip_with_email()
    {
        using var scope = new AssertionScope();
        var trip = new CreateTrip
        {
            Name = "Trip 1",
            Description = "Trip 1 description\n",
            Country = "Poland",
            StartDate = DateTimeOffset.Now,
            SeatsCount = 21
        };
        var createTrip = await _fixture.Server.CreateHttpApiRequest<TripsController>(c => c.CreateTripAsync(trip))
                                           .PostAsync();
        var tripId = (await createTrip.Content.ReadFromJsonAsync<Envelope<Trip>>()).Result.Id;
        var email = "a@b";
        
        var registerResponse = await _fixture.Server.CreateHttpApiRequest<RegistrationsController>(c => c.RegisterAsync(new Register { TripId = tripId, EmailAddress = email }))
                                           .PostAsync();
        registerResponse.Should().Be200Ok();
        
        registerResponse = await _fixture.Server.CreateHttpApiRequest<RegistrationsController>(c => c.RegisterAsync(new Register { TripId = tripId, EmailAddress = email }))
                                             .PostAsync();
        registerResponse.Should().Be400BadRequest();
        
        var unregisterResponse = await _fixture.Server.CreateHttpApiRequest<RegistrationsController>(c => c.UnregisterAsync(new Unregister { TripId = tripId, EmailAddress = email }))
                                             .SendAsync("DELETE");
        unregisterResponse.Should().Be200Ok();
        
        unregisterResponse = await _fixture.Server.CreateHttpApiRequest<RegistrationsController>(c => c.UnregisterAsync(new Unregister { TripId = tripId, EmailAddress = email }))
                                             .SendAsync("DELETE");
        unregisterResponse.Should().Be400BadRequest();
        
        var deleteResponse = await _fixture.Server.CreateHttpApiRequest<TripsController>(c => c.DeleteTripAsync(tripId))
                                           .SendAsync("DELETE");
    }
}