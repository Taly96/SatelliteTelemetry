namespace SatelliteTelemetryAPI.Infrastructure.Persistence.Entities;

public record AlertEntity(string SatelliteId, string Message, DateTime CreatedAt);