using SatelliteTelemetryAPI.API.DTOs;
using SatelliteTelemetryAPI.Core.Interfaces;

namespace SatelliteTelemetryAPI.Core.Services;

public class SatelliteTelemetryService : ITelemetryService
{
    public async Task<bool> AddTelemetriesToRecordAsync(IReadOnlyList<TelemetryDTO> records)
    {
        throw new NotImplementedException();
    }
}