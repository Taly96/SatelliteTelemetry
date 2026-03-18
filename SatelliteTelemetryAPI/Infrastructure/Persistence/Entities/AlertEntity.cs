using SatelliteTelemetryAPI.Core.Enums;

namespace SatelliteTelemetryAPI.Infrastructure.Persistence.Entities;

public record AlertEntity(Guid SatelliteId, AlertType AlertType, string Message, DateTime CreatedAt);