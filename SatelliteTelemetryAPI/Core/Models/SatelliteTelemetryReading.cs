namespace SatelliteTelemetryAPI.Core.Models;

public record SatelliteTelemetryReading(
    string SatelliteId,
    DateTime Timestamp,
    double BatteryLevel,
    double Temperature);