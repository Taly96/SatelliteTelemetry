using System.Collections.Concurrent;
using SatelliteTelemetryAPI.Core.Interfaces;
using SatelliteTelemetryAPI.Infrastructure.Persistence.Entities;

namespace SatelliteTelemetryAPI.Infrastructure.Persistence.Repositories;

public class InMemoryTelemetryRepository : ITelemetryRepository
{
    private readonly ConcurrentQueue<AlertEntity> _alerts = new();
    private readonly ILogger<InMemoryTelemetryRepository> _logger;
    private readonly ConcurrentDictionary<string, ConcurrentQueue<SatelliteTelemetryEntity>> _storage = new();

    public InMemoryTelemetryRepository(ILogger<InMemoryTelemetryRepository> logger)
    {
        _logger = logger;
    }

    public Task AddReadingsAsync(IEnumerable<SatelliteTelemetryEntity> telemetryEntities)
    {
        _logger.LogInformation("Adding readings to inMemory storage");

        foreach (var entity in telemetryEntities)
        {
            var queue = _storage.GetOrAdd(entity.SatelliteId, _ => new ConcurrentQueue<SatelliteTelemetryEntity>());

            queue.Enqueue(entity);
        }

        _logger.LogInformation("Added readings to inMemory storage successfully");

        return Task.CompletedTask;
    }

    public Task<IEnumerable<SatelliteTelemetryEntity>> GetRecentReadingsAsync(string satelliteId, int count)
    {
        _logger.LogInformation("Getting recent readings from inMemory storage");

        if (!_storage.TryGetValue(satelliteId, out var queue))
        {
            _logger.LogInformation($"Satellite {satelliteId} not found, cannot get readings.");

            return Task.FromResult(Enumerable.Empty<SatelliteTelemetryEntity>());
        }

        var recentReadings = queue.OrderByDescending(satelliteTelemetryEntity => satelliteTelemetryEntity.Timestamp)
            .Take(count).ToList();

        _logger.LogInformation("Retrieved recent readings from inMemory storage successfully");

        return Task.FromResult<IEnumerable<SatelliteTelemetryEntity>>(recentReadings);
    }

    public Task AddAlertAsync(AlertEntity alert)
    {
        _logger.LogInformation("Adding alert to inMemory storage");
        _alerts.Enqueue(alert);
        _logger.LogInformation("Added alert to inMemory storage successfully");

        return Task.CompletedTask;
    }

    public Task<IEnumerable<AlertEntity>> GetAllAlertsAsync()
    {
        _logger.LogInformation("Getting all alerts");

        return Task.FromResult<IEnumerable<AlertEntity>>(_alerts.ToArray());
    }
}