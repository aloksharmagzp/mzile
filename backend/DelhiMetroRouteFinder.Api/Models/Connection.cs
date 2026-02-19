namespace DelhiMetroRouteFinder.Api.Models;

public class Connection
{
    public int ConnectionId { get; set; }
    public int FromStationId { get; set; }
    public int ToStationId { get; set; }
    public decimal DistanceKm { get; set; }
    public int TimeMinutes { get; set; }

    public Station? FromStation { get; set; }
    public Station? ToStation { get; set; }
}
