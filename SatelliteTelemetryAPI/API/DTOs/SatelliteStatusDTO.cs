namespace SatelliteTelemetryAPI.API.DTOs;

public record SatelliteStatusDTO(
    Guid SatelliteId,
    SatelliteTelemetryDTO LastReading,
    double AverageTemperature);