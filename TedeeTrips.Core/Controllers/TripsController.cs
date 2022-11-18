using MediatR;
using Microsoft.AspNetCore.Mvc;
using TedeeTrips.Application.Query;
using TedeeTrips.Core.Presentation;
using TedeeTrips.Domain.Commands;
using TedeeTrips.Domain.Entities;

namespace TedeeTrips.Core.Controllers;

[ApiController]
[Route("/api/trips")]
public class TripsController : ControllerBase
{
    private readonly IMediator _commandRelay;

    public TripsController(IMediator commandRelay)
    {
        _commandRelay = commandRelay;
    }

    [HttpPost]
    public async Task<ActionResult<Envelope<Presentation.Trip>>> PostAsync(CreateTrip command)
    {
        var res = await _commandRelay.Send(command);

        if (res.IsFailure)
        {
            return BadRequest(Envelope.Error(res.Error));
        }
        
        // we could return just an ID, but for the sake of the evaluation, we return the whole object
        return CreatedAtRoute("GetTrip", new { id = res.Value.Id }, Presentation.Trip.From(res.Value));
    }
    
    [HttpGet("{id:guid}", Name = "GetTrip")]
    public async Task<ActionResult<Envelope<Presentation.Trip>>> GetSingleTripAsync([FromRoute] Guid id)
    {
        var res = await _commandRelay.Send(new GetTrip(id));

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
            res = await _commandRelay.Send(new GetTripsByCountry(country));
        }
        else
        {
            res = await _commandRelay.Send(new GetTrips());
        }
        
        var returnables = res.Select(t => Presentation.DescriptionlessTrip.From(t)).ToList();
        
        return Ok(returnables);
    }
}