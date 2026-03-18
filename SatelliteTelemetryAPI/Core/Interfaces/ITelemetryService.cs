using SatelliteTelemetryAPI.API.DTOs;
using SatelliteTelemetryAPI.Core.Models;

namespace SatelliteTelemetryAPI.Core.Interfaces;

public interface ITelemetryService
{
    Task AddTelemetriesToRecordAsync(IReadOnlyList<SatelliteTelemetryDTO> records);
    Task<SatelliteStatusDTO?> GetSatelliteStatusAsync(Guid satelliteId);
    Task<IReadOnlyList<AlertDTO>> GetActiveAlertsAsync(AlertFiltersDTO filters);
}