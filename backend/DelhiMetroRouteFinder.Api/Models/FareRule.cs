namespace DelhiMetroRouteFinder.Api.Models;

public class FareRule
{
    public int FareRuleId { get; set; }
    public int MinStations { get; set; }
    public int MaxStations { get; set; }
    public decimal Fare { get; set; }
}
