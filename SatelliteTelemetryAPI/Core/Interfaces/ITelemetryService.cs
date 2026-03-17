using SatelliteTelemetryAPI.API.DTOs;

namespace SatelliteTelemetryAPI.Core.Interfaces;

public interface ITelemetryService
{
    Task<bool> AddTelemetriesToRecordAsync(IReadOnlyList<TelemetryDTO> records);
}