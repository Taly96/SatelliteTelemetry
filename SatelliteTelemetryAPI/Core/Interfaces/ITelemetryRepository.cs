using SatelliteTelemetryAPI.Infrastructure.Persistence.Entities;

namespace SatelliteTelemetryAPI.Core.Interfaces;

public interface ITelemetryRepository
{
    Task AddReadingsAsync(IEnumerable<SatelliteTelemetryEntity> entities);
    Task<IEnumerable<SatelliteTelemetryEntity>> GetRecentReadingsAsync(string satelliteId, int count);
    Task AddAlertAsync(AlertEntity alert);
    Task<IEnumerable<AlertEntity>> GetAllAlertsAsync();
}