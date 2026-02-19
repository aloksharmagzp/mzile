using DelhiMetroRouteFinder.Api.Models;

namespace DelhiMetroRouteFinder.Api.Services;

public interface IRouteService
{
    Task<RouteResponse?> FindBestRouteAsync(int sourceStationId, int destinationStationId);
    Task<decimal?> CalculateFareAsync(int stationCount);
}
