using SatelliteTelemetryAPI.API.DTOs;
using SatelliteTelemetryAPI.Core.Models;

namespace SatelliteTelemetryAPI.Core.Interfaces;

public interface ITelemetryService
{
    Task<bool> AddTelemetriesToRecordAsync(IReadOnlyList<SatelliteTelemetryDTO> records);
    Task<SatelliteStatusDTO?> GetSatelliteStatusAsync(string satelliteId);
    Task<IReadOnlyList<AlertDTO>> GetActiveAlertsAsync();
}