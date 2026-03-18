using System.Collections.Concurrent;
using SatelliteTelemetryAPI.API.DTOs;
using SatelliteTelemetryAPI.Core.Interfaces;
using SatelliteTelemetryAPI.Infrastructure.Persistence.Entities;

namespace SatelliteTelemetryAPI.Infrastructure.Persistence.Repositories;

public class InMemoryTelemetryRepository : ITelemetryRepository
{
    private readonly ConcurrentQueue<AlertEntity> _activeAlerts = new();
    private readonly ILogger<InMemoryTelemetryRepository> _logger;
    private readonly ConcurrentDictionary<Guid, ConcurrentQueue<SatelliteTelemetryEntity>> _telemetryStorage = new();

    public InMemoryTelemetryRepository(ILogger<InMemoryTelemetryRepository> logger)
    {
        _logger = logger;
    }

    public Task AddTelemetryReadingsAsync(IEnumerable<SatelliteTelemetryEntity> telemetryEntities)
    {
        _logger.LogInformation("Adding readings to inMemory storage");

        foreach (var telemetryEntity in telemetryEntities)
        {
            var telemetryEntitiesQueue = _telemetryStorage.GetOrAdd(telemetryEntity.SatelliteId,
                _ => new ConcurrentQueue<SatelliteTelemetryEntity>());

            telemetryEntitiesQueue.Enqueue(telemetryEntity);
        }

        _logger.LogInformation("Added readings to inMemory storage successfully");

        return Task.CompletedTask;
    }

    public Task AddAlertAsync(AlertEntity newAlert)
    {
        _logger.LogInformation("Adding alert to inMemory storage");
        _activeAlerts.Enqueue(newAlert);
        _logger.LogInformation("Added alert to inMemory storage successfully");

        return Task.CompletedTask;
    }

    public Task<IEnumerable<AlertEntity>> GetActiveFilteredAlertsAsync(AlertFiltersDTO filters)
    {
        _logger.LogInformation(
            "Fetching alerts with filters: SatelliteId={SatelliteId}, FromDate={FromDate}, Page={PageNumber}, PageSize={PageSize}",
            filters.SatelliteId, filters.FromDate, filters.PageNumber, filters.PageSize);

        var startTime = DateTime.UtcNow;
        var activeFiltersQuery = _activeAlerts.AsEnumerable();

        if (filters.SatelliteId.HasValue && filters.SatelliteId != Guid.Empty)
        {
            _logger.LogDebug("Filtering by SatelliteId: {SatelliteId}", filters.SatelliteId);
            activeFiltersQuery =
                activeFiltersQuery.Where(alertEntity => alertEntity.SatelliteId == filters.SatelliteId.Value);
        }

        if (filters.FromDate.HasValue)
        {
            _logger.LogDebug("Filtering by FromDate: {FromDate}", filters.FromDate);
            activeFiltersQuery =
                activeFiltersQuery.Where(alertEntity => alertEntity.CreatedAt >= filters.FromDate.Value);
        }

        var pagedResult = activeFiltersQuery
            .OrderByDescending(alertEntity => alertEntity.CreatedAt)
            .Skip((filters.PageNumber - 1) * filters.PageSize)
            .Take(filters.PageSize)
            .ToList();

        var elapsedMs = (DateTime.UtcNow - startTime).TotalMilliseconds;

        _logger.LogInformation("Successfully retrieved {Count} alerts. Query took {ElapsedMs}ms",
            pagedResult.Count, elapsedMs);

        return Task.FromResult<IEnumerable<AlertEntity>>(pagedResult);
    }

    public Task<IEnumerable<SatelliteTelemetryEntity>> GetRecentTelemetryReadingsAsync(Guid satelliteId, int count)
    {
        _logger.LogInformation("Getting recent readings from inMemory storage for {SatelliteId}", satelliteId);

        if (!_telemetryStorage.TryGetValue(satelliteId, out var telemetriesQueue) || telemetriesQueue.IsEmpty)
        {
            _logger.LogInformation("Satellite {SatelliteId} not found or has no readings.", satelliteId);

            return Task.FromResult(Enumerable.Empty<SatelliteTelemetryEntity>());
        }

        var recentReadings = telemetriesQueue.Reverse().Take(count).ToList();

        _logger.LogInformation("Retrieved {Count} recent readings for satellite {SatelliteId}", recentReadings.Count,
            satelliteId);

        return Task.FromResult<IEnumerable<SatelliteTelemetryEntity>>(recentReadings);
    }
}
