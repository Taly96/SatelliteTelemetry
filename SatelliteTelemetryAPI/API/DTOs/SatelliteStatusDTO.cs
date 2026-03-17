namespace SatelliteTelemetryAPI.API.DTOs;

public record SatelliteStatusDTO(
    string SatelliteId,
    SatelliteTelemetryDTO LastReading,
    double AverageTemperature);