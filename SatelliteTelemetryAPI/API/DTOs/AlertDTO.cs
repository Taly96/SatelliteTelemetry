using SatelliteTelemetryAPI.Core.Enums;

namespace SatelliteTelemetryAPI.Core.Models;

public record AlertDTO(
    string SatelliteId,
    string Reason,
    AlertType AlertType,
    DateTime Timestamp);