using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TedeeTrips.Core.Controllers;
using TedeeTrips.Core.Presentation;
using TedeeTrips.Domain.Commands;
using TedeeTrips.Tests.Fixtures;
using Xunit;

namespace TedeeTrips.Tests.Integration;

[Collection(nameof(ApiCollection))]
public class TripTests : IClassFixture<TestHostFixture>
{
    private readonly TestHostFixture _fixture;
    
    public TripTests(TestHostFixture fixture)
    {
        _fixture = fixture;
    }
    
    public static TheoryData<List<CreateTrip>> CreateTripData() =>
        new()
        {
            new List<CreateTrip>()
            {
                new CreateTrip
                {
                    Name = "Trip 1",
                    Description = "Trip 1 description\n",
                    Country = "Poland",
                    StartDate = DateTimeOffset.Now,
                    SeatsCount = 21
                },
                new CreateTrip
                {
                    Name = "Trip 2",
                    Description = "Trip 2 description\r\n",
                    Country = "Mexico",
                    StartDate = DateTimeOffset.Now,
                    SeatsCount = 37
                }
            }
        };

    [Theory]
    [MemberData(nameof(CreateTripData))]
    public async Task Trip_is_created_and_retrieved_and_modified_and_deleted(List<CreateTrip> list)
    {
        using var scope = new AssertionScope();
        var request = list.First();
        
        var createResponse = await _fixture.Server.CreateHttpApiRequest<TripsController>(c => c.CreateTripAsync(request))
                               .PostAsync();
        var createResponseBody = await createResponse.Content.ReadFromJsonAsync<Envelope<Trip>>();
        createResponse.Should().Be201Created();
        
        var getResponse = await _fixture.Server.CreateHttpApiRequest<TripsController>(c => c.GetTripAsync(createResponseBody.Result.Id))
                                        .GetAsync();

        getResponse.Should().Be200Ok();
        
        var getResponseBody = await getResponse.Content.ReadFromJsonAsync<Envelope<Trip>>();

        createResponse.Should().Be201Created();
        getResponseBody.Result.Name.Should().Be(request.Name);
        getResponseBody.Result.Description.Should().Be(request.Description);
        getResponseBody.Result.StartDate.Should().Be(request.StartDate);
        getResponseBody.Result.SeatsCount.Should().Be(request.SeatsCount);

        var modifyBase = list.Last();
        var modifyRequest = new ModifyTripRequest() { Name = modifyBase.Name, Description = modifyBase.Description, StartDate = modifyBase.StartDate, SeatsCount = modifyBase.SeatsCount };
        
        var updateResponse = await _fixture.Server.CreateHttpApiRequest<TripsController>(c => c.ModifyTripAsync(createResponseBody.Result.Id, modifyRequest))
                                           .SendAsync("PATCH");

        updateResponse.Should().Be200Ok();
        
        var deleteResponse = await _fixture.Server.CreateHttpApiRequest<TripsController>(c => c.DeleteTripAsync(createResponseBody.Result.Id))
                                           .SendAsync("DELETE");
        deleteResponse.Should().Be200Ok();
    }
    
    [Theory]
    [MemberData(nameof(CreateTripData))]
    public async Task Trip_name_must_be_unique(List<CreateTrip> list)
    {
        using var scope = new AssertionScope();
        var request = list.First();
        
        var createResponse1 = await _fixture.Server.CreateHttpApiRequest<TripsController>(c => c.CreateTripAsync(request))
                                           .PostAsync();
        var createResponseBody1 = await createResponse1.Content.ReadFromJsonAsync<Envelope<Trip>>();
        createResponse1.Should().Be201Created();
        
        var createResponse2 = await _fixture.Server.CreateHttpApiRequest<TripsController>(c => c.CreateTripAsync(request))
                                            .PostAsync();
        createResponse2.Should().Be400BadRequest();

        var deleteResponse = await _fixture.Server.CreateHttpApiRequest<TripsController>(c => c.DeleteTripAsync(createResponseBody1.Result.Id))
                                           .SendAsync("DELETE");
        deleteResponse.Should().Be200Ok();
    }
}