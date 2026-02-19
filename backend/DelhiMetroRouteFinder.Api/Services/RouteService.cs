using DelhiMetroRouteFinder.Api.Data;
using DelhiMetroRouteFinder.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DelhiMetroRouteFinder.Api.Services;

public class RouteService(MetroDbContext dbContext) : IRouteService
{
    public async Task<RouteResponse?> FindBestRouteAsync(int sourceStationId, int destinationStationId)
    {
        if (sourceStationId == destinationStationId)
        {
            var station = await dbContext.Stations
                .Include(s => s.Line)
                .FirstOrDefaultAsync(s => s.StationId == sourceStationId);

            if (station is null)
            {
                return null;
            }

            return new RouteResponse
            {
                Stations =
                [
                    new RouteStationDto
                    {
                        StationId = station.StationId,
                        Name = station.Name,
                        LineName = station.Line?.LineName ?? string.Empty,
                        Color = station.Line?.Color ?? "#000000"
                    }
                ],
                TotalStations = 1,
                TotalTimeMinutes = 0,
                TotalDistanceKm = 0,
                Fare = 0,
                FirstMetroTime = station.FirstMetroTime,
                LastMetroTime = station.LastMetroTime
            };
        }

        var stations = await dbContext.Stations.Include(s => s.Line).ToListAsync();
        var stationById = stations.ToDictionary(s => s.StationId);

        if (!stationById.ContainsKey(sourceStationId) || !stationById.ContainsKey(destinationStationId))
        {
            return null;
        }

        var edges = await dbContext.Connections.ToListAsync();
        var adjacency = edges.GroupBy(c => c.FromStationId)
            .ToDictionary(g => g.Key, g => g.ToList());

        var distance = stations.ToDictionary(s => s.StationId, _ => int.MaxValue);
        var previous = new Dictionary<int, int?>();
        var pq = new PriorityQueue<int, int>();

        foreach (var station in stations)
        {
            previous[station.StationId] = null;
        }

        distance[sourceStationId] = 0;
        pq.Enqueue(sourceStationId, 0);

        while (pq.Count > 0)
        {
            pq.TryDequeue(out var current, out var currentCost);
            if (currentCost > distance[current])
            {
                continue;
            }

            if (current == destinationStationId)
            {
                break;
            }

            if (!adjacency.TryGetValue(current, out var neighbors))
            {
                continue;
            }

            foreach (var edge in neighbors)
            {
                var newCost = distance[current] + edge.TimeMinutes;
                if (newCost < distance[edge.ToStationId])
                {
                    distance[edge.ToStationId] = newCost;
                    previous[edge.ToStationId] = current;
                    pq.Enqueue(edge.ToStationId, newCost);
                }
            }
        }

        if (distance[destinationStationId] == int.MaxValue)
        {
            return null;
        }

        var pathStationIds = new List<int>();
        var cursor = destinationStationId;

        while (true)
        {
            pathStationIds.Add(cursor);
            var prev = previous[cursor];
            if (!prev.HasValue)
            {
                break;
            }

            cursor = prev.Value;
        }

        pathStationIds.Reverse();

        decimal totalDistanceKm = 0;
        for (var i = 0; i < pathStationIds.Count - 1; i++)
        {
            var from = pathStationIds[i];
            var to = pathStationIds[i + 1];
            var edge = edges.FirstOrDefault(c => c.FromStationId == from && c.ToStationId == to);
            if (edge is not null)
            {
                totalDistanceKm += edge.DistanceKm;
            }
        }

        var routeStations = pathStationIds.Select(id =>
        {
            var station = stationById[id];
            return new RouteStationDto
            {
                StationId = station.StationId,
                Name = station.Name,
                LineName = station.Line?.LineName ?? string.Empty,
                Color = station.Line?.Color ?? "#000000"
            };
        }).ToList();

        var interchangeStations = new List<string>();
        for (var i = 1; i < routeStations.Count - 1; i++)
        {
            var prevLine = routeStations[i - 1].LineName;
            var currentLine = routeStations[i].LineName;
            var nextLine = routeStations[i + 1].LineName;
            if (currentLine != prevLine || currentLine != nextLine)
            {
                if (prevLine != nextLine && !interchangeStations.Contains(routeStations[i].Name))
                {
                    interchangeStations.Add(routeStations[i].Name);
                }
            }
        }

        var stationCount = pathStationIds.Count;
        var fare = await CalculateFareAsync(stationCount) ?? 0;

        return new RouteResponse
        {
            Stations = routeStations,
            TotalStations = stationCount,
            TotalTimeMinutes = distance[destinationStationId],
            TotalDistanceKm = Math.Round(totalDistanceKm, 2),
            Fare = fare,
            InterchangeStations = interchangeStations,
            FirstMetroTime = stationById[sourceStationId].FirstMetroTime,
            LastMetroTime = stationById[destinationStationId].LastMetroTime
        };
    }

    public async Task<decimal?> CalculateFareAsync(int stationCount)
    {
        var rule = await dbContext.FareRules
            .OrderBy(f => f.MinStations)
            .FirstOrDefaultAsync(f => stationCount >= f.MinStations && stationCount <= f.MaxStations);

        return rule?.Fare;
    }
}
