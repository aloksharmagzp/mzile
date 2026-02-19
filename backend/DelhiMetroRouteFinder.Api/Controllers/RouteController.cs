using DelhiMetroRouteFinder.Api.Models;
using DelhiMetroRouteFinder.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace DelhiMetroRouteFinder.Api.Controllers;

[ApiController]
[Route("api/route")]
public class RouteController(IRouteService routeService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> FindRoute([FromBody] RouteRequest request)
    {
        if (request.SourceStationId <= 0 || request.DestinationStationId <= 0)
        {
            return BadRequest("Source and destination station IDs are required.");
        }

        var route = await routeService.FindBestRouteAsync(request.SourceStationId, request.DestinationStationId);
        if (route is null)
        {
            return NotFound("No route found for the given stations.");
        }

        return Ok(route);
    }
}
