using System.ComponentModel.DataAnnotations;
using SatelliteTelemetryAPI.API.Attributes;

namespace SatelliteTelemetryAPI.API.DTOs;

public record AlertFiltersDTO
{
    [NotEmptyGuid] public Guid? SatelliteId { get; init; }

    public DateTime? FromDate { get; init; }

    [Range(1, int.MaxValue, ErrorMessage = "Page number must be greater than 0.")]
    public int PageNumber { get; init; } = 1;

    [Range(1, 100, ErrorMessage = "Page size must be between 1 and 100.")]
    public int PageSize { get; init; } = 10;
}
