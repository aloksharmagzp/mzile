namespace DelhiMetroRouteFinder.Api.Models;

public class Line
{
    public int LineId { get; set; }
    public string LineName { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;

    public ICollection<Station> Stations { get; set; } = new List<Station>();
}
