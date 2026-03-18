using SatelliteTelemetryAPI.API.DTOs;
using SatelliteTelemetryAPI.Infrastructure.Persistence.Entities;

namespace SatelliteTelemetryAPI.Core.Interfaces;

public interface ITelemetryRepository
{
    Task AddTelemetryReadingsAsync(IEnumerable<SatelliteTelemetryEntity> entities);

    Task<IEnumerable<SatelliteTelemetryEntity>> GetRecentTelemetryReadingsAsync(Guid satelliteId, int count);

    Task AddAlertAsync(AlertEntity alert);

    Task<IEnumerable<AlertEntity>> GetActiveFilteredAlertsAsync(AlertFiltersDTO filters);
}