using MediatR;
using Microsoft.AspNetCore.Mvc;
using TedeeTrips.Core.Presentation;
using TedeeTrips.Domain.Commands;

namespace TedeeTrips.Core.Controllers;

[Route("api/registrations")]
[ApiController]
public class RegistrationsController : ControllerBase
{
    private readonly IMediator _relay;

    public RegistrationsController(IMediator relay)
    {
        _relay = relay;
    }
    
    [HttpPost]
    public async Task<ActionResult<Envelope>> RegisterAsync(Register command)
    {
        var res = await _relay.Send(command);

        if (res.IsFailure)
        {
            return BadRequest(Envelope.Error(res.Error));
        }

        return Ok();
    }
    
    [HttpDelete]
    public async Task<ActionResult<Envelope>> UnregisterAsync(Unregister command)
    {
        var res = await _relay.Send(command);

        if (res.IsFailure)
        {
            return BadRequest(Envelope.Error(res.Error));
        }

        return Ok();
    }
}