namespace DelhiMetroRouteFinder.Api.Models;

public class RouteResponse
{
    public List<RouteStationDto> Stations { get; set; } = new();
    public int TotalStations { get; set; }
    public int TotalTimeMinutes { get; set; }
    public decimal TotalDistanceKm { get; set; }
    public decimal Fare { get; set; }
    public List<string> InterchangeStations { get; set; } = new();
    public int InterchangeCount => InterchangeStations.Count;
    public TimeOnly FirstMetroTime { get; set; }
    public TimeOnly LastMetroTime { get; set; }
}

public class RouteStationDto
{
    public int StationId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string LineName { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
}
