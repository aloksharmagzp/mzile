using DelhiMetroRouteFinder.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace DelhiMetroRouteFinder.Api.Controllers;

[ApiController]
[Route("api/fare")]
public class FareController(IRouteService routeService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetFare([FromQuery] int stationCount)
    {
        if (stationCount <= 0)
        {
            return BadRequest("stationCount must be greater than zero.");
        }

        var fare = await routeService.CalculateFareAsync(stationCount);
        if (!fare.HasValue)
        {
            return NotFound("No fare slab found for the given station count.");
        }

        return Ok(new { stationCount, fare = fare.Value });
    }
}
