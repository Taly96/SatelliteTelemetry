using SatelliteTelemetryAPI.API.DTOs;
using SatelliteTelemetryAPI.Core.Interfaces;
using SatelliteTelemetryAPI.Core.Models;

namespace SatelliteTelemetryAPI.Core.Services;

public class SatelliteTelemetryService : ITelemetryService
{
    public async Task<bool> AddTelemetriesToRecordAsync(IReadOnlyList<SatelliteTelemetryDTO> records)
    {
        throw new NotImplementedException();
    }

    public Task<SatelliteStatusDTO?> GetSatelliteStatusAsync(string satelliteId)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<AlertDTO>> GetActiveAlertsAsync()
    {
        throw new NotImplementedException();
    }
}