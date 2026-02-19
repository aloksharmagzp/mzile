namespace DelhiMetroRouteFinder.Api.Models;

public class Station
{
    public int StationId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int LineId { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public TimeOnly FirstMetroTime { get; set; }
    public TimeOnly LastMetroTime { get; set; }

    public Line? Line { get; set; }
    public ICollection<Connection> OutgoingConnections { get; set; } = new List<Connection>();
    public ICollection<Connection> IncomingConnections { get; set; } = new List<Connection>();
}
