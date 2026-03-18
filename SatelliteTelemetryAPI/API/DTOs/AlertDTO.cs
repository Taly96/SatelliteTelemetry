using SatelliteTelemetryAPI.Core.Enums;

namespace SatelliteTelemetryAPI.Core.Models;

public record AlertDTO(
    Guid SatelliteId,
    string Reason,
    AlertType AlertType,
    DateTime Timestamp);