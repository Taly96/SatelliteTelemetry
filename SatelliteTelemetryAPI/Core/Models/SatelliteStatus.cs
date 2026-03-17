namespace SatelliteTelemetryAPI.Core.Models;

public record SatelliteStatus(
    string SatelliteId,
    SatelliteTelemetryReading LastReading,
    double AverageTemperature);