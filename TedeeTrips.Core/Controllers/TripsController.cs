using MediatR;
using Microsoft.AspNetCore.Mvc;
using TedeeTrips.Application.Query;
using TedeeTrips.Core.Presentation;
using TedeeTrips.Domain.Commands;

namespace TedeeTrips.Core.Controllers;

[ApiController]
[Route("/api/trips")]
public class TripsController : ControllerBase
{
    private readonly IMediator _relay;

    public TripsController(IMediator relay)
    {
        _relay = relay;
    }

    [HttpPost]
    public async Task<ActionResult<Envelope<Presentation.Trip>>> PostAsync(CreateTrip command)
    {
        var res = await _relay.Send(command);

        if (res.IsFailure)
        {
            return BadRequest(Envelope.Error(res.Error));
        }
        
        // we could return just an ID, but for the sake of the evaluation, we return the whole object
        return CreatedAtRoute("GetTrip", new { id = res.Value.Id }, Presentation.Trip.From(res.Value));
    }
    
    [HttpGet("{id:guid}", Name = "GetTrip")]
    public async Task<ActionResult<Envelope<Presentation.Trip>>> GetTripAsync([FromRoute] Guid id)
    {
        var res = await _relay.Send(new GetTrip(id));

        if (res.TryGetValue(out var trip))
        {
            return Ok(Envelope.Ok(Presentation.Trip.From(trip)));
        }

        return NotFound();
    }
    
    [HttpGet("", Name = "GetTripsByCountry")]
    public async Task<ActionResult<Envelope<Presentation.DescriptionlessTrip>>> GetTripsByCountryAsync([FromQuery] string? country)
    {
        ICollection<Domain.Entities.Trip> res;

        if (!string.IsNullOrWhiteSpace(country))
        {
            res = await _relay.Send(new GetTripsByCountry(country));
        }
        else
        {
            res = await _relay.Send(new GetTrips());
        }
        
        var returnables = res.Select(Presentation.DescriptionlessTrip.From).ToList();
        
        return Ok(Envelope.Ok(returnables));
    }
    
    [HttpDelete("{id:guid}", Name = "DeleteTrip")]
    public async Task<ActionResult<Envelope>> DeleteTripAsync([FromRoute] Guid id)
    {
        await _relay.Send(new DeleteTrip(id));
        return Ok(Envelope.Ok());
    }
    
    [HttpPatch("{id:guid}", Name = "ModifyTrip")]
    public async Task<ActionResult<Envelope<Presentation.Trip>>> ModifyTripAsync([FromRoute] Guid id, [FromBody] ModifyTripRequest request)
    {
        var command = new ModifyTrip()
        {
            Id = id,
            Name = request.Name,
            Description = request.Description,
            CountryId = request.CountryId,
            StartDate = request.StartDate,
            SeatsCount = request.SeatsCount
        };
        
        var res = await _relay.Send(command);

        if (res.IsFailure)
        {
            return BadRequest(Envelope.Error(res.Error));
        }
        
        return Ok(Envelope.Ok());
    }
}