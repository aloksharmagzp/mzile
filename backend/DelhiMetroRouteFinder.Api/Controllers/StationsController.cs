using DelhiMetroRouteFinder.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DelhiMetroRouteFinder.Api.Controllers;

[ApiController]
[Route("api/stations")]
public class StationsController(MetroDbContext dbContext) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetStations()
    {
        var stations = await dbContext.Stations
            .Include(s => s.Line)
            .OrderBy(s => s.Name)
            .Select(s => new
            {
                s.StationId,
                s.Name,
                s.LineId,
                LineName = s.Line!.LineName,
                LineColor = s.Line!.Color,
                s.Latitude,
                s.Longitude,
                s.FirstMetroTime,
                s.LastMetroTime
            })
            .ToListAsync();

        return Ok(stations);
    }
}
