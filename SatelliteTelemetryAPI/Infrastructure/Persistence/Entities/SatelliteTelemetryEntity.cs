namespace SatelliteTelemetryAPI.Infrastructure.Persistence.Entities;

public record SatelliteTelemetryEntity(Guid SatelliteId, DateTime Timestamp, double BatteryLevel, double Temperature);