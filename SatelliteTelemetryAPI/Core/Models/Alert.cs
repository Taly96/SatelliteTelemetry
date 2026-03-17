using SatelliteTelemetryAPI.Core.Enums;

namespace SatelliteTelemetryAPI.Core.Models;

public record Alert(
    string SatelliteId,
    string Reason,
    AlertType AlertType,
    DateTime Timestamp);