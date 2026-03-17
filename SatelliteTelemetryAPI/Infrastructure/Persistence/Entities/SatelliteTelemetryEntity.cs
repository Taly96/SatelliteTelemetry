namespace SatelliteTelemetryAPI.Infrastructure.Persistence.Entities;

public record SatelliteTelemetryEntity(string SatelliteId, DateTime Timestamp, double BatteryLevel, double Temperature);