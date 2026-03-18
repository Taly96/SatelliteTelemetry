using System.ComponentModel.DataAnnotations;
using SatelliteTelemetryAPI.API.Attributes;

namespace SatelliteTelemetryAPI.API.DTOs;

public record SatelliteTelemetryDTO
{
    [Required] [NotEmptyGuid] public required Guid SatelliteId { get; init; }

    [Required] public required DateTime Timestamp { get; init; }

    [Required]
    [Range(0, 100, ErrorMessage = "Battery level must be between 0 and 100 percent.")]
    public required double BatteryLevel { get; init; }

    [Required]
    [Range(-273.15, 1000, ErrorMessage = "Temperature is outside of physically possible satellite limits.")]
    public required double Temperature { get; init; }
}