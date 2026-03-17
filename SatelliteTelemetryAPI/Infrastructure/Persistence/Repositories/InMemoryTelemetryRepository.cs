using System.Collections.Concurrent;
using SatelliteTelemetryAPI.Core.Interfaces;
using SatelliteTelemetryAPI.Infrastructure.Persistence.Entities;

namespace SatelliteTelemetryAPI.Infrastructure.Persistence.Repositories;

public class InMemoryTelemetryRepository : ITelemetryRepository
{
    private readonly ConcurrentQueue<AlertEntity> _alerts = new();
    private readonly ConcurrentDictionary<string, ConcurrentQueue<SatelliteTelemetryEntity>> _storage = new();

    public Task AddReadingsAsync(IEnumerable<SatelliteTelemetryEntity> entities)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<SatelliteTelemetryEntity>> GetRecentReadingsAsync(string satelliteId, int count)
    {
        throw new NotImplementedException();
    }

    public Task AddAlertAsync(AlertEntity alert)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<AlertEntity>> GetAllAlertsAsync()
    {
        throw new NotImplementedException();
    }
}