using SatelliteTelemetryAPI.API.DTOs;
using SatelliteTelemetryAPI.Core.Enums;
using SatelliteTelemetryAPI.Core.Interfaces;
using SatelliteTelemetryAPI.Core.Models;
using SatelliteTelemetryAPI.Infrastructure.Persistence.Entities;

namespace SatelliteTelemetryAPI.Core.Services;

public class SatelliteTelemetryService : ITelemetryService
{
    private readonly int _criticalBatteryThreshold = 20;

    private readonly int _criticalTemperatureThreshold = 80;

    private readonly ILogger<SatelliteTelemetryService> _logger;

    private readonly ITelemetryRepository _satelliteTelemetryRepository;

    public SatelliteTelemetryService(ITelemetryRepository repository, ILogger<SatelliteTelemetryService> logger)
    {
        _satelliteTelemetryRepository = repository;
        _logger = logger;
    }

    public async Task AddTelemetriesToRecordAsync(IReadOnlyList<SatelliteTelemetryDTO> records)
    {
        _logger.LogInformation("Processing {Count} telemetry records.", records.Count);

        var satelliteTelemetryEntities = records.Select(satelliteTelemetryRecord => new SatelliteTelemetryEntity(
            satelliteTelemetryRecord.SatelliteId,
            satelliteTelemetryRecord.Timestamp,
            satelliteTelemetryRecord.BatteryLevel,
            satelliteTelemetryRecord.Temperature)).ToList();

        await _satelliteTelemetryRepository.AddTelemetryReadingsAsync(satelliteTelemetryEntities);
        var alertTasks = new List<Task>();

        foreach (var satelliteTelemetryEntity in satelliteTelemetryEntities)
        {
            if (satelliteTelemetryEntity.BatteryLevel < _criticalBatteryThreshold)
            {
                alertTasks.Add(_satelliteTelemetryRepository.AddAlertAsync(new AlertEntity(
                    satelliteTelemetryEntity.SatelliteId, AlertType.BatteryDrop,
                    $"Critical Battery: {satelliteTelemetryEntity.BatteryLevel}%",
                    DateTime.UtcNow)));
            }

            if (satelliteTelemetryEntity.Temperature > _criticalTemperatureThreshold)
            {
                alertTasks.Add(_satelliteTelemetryRepository.AddAlertAsync(new AlertEntity(
                    satelliteTelemetryEntity.SatelliteId, AlertType.TemperatureSpike,
                    $"High Temp: {satelliteTelemetryEntity.Temperature} °C",
                    DateTime.UtcNow)));
            }
        }

        if (alertTasks.Count != 0)
        {
            await Task.WhenAll(alertTasks);
        }
    }

    public async Task<SatelliteStatusDTO?> GetSatelliteStatusAsync(Guid satelliteId)
    {
        _logger.LogDebug("Calculating status for satellite: {Id}", satelliteId);

        var recentReadings = (await _satelliteTelemetryRepository.GetRecentTelemetryReadingsAsync(satelliteId, 10))
            .ToList();

        if (recentReadings.Count == 0)
        {
            _logger.LogWarning("No readings found for satellite: {Id}", satelliteId);

            return null;
        }

        var latest = recentReadings.First();
        var avgTemp = recentReadings.Average(satelliteTelemetryEntity => satelliteTelemetryEntity.Temperature);

        return new SatelliteStatusDTO(
            satelliteId,
            new SatelliteTelemetryDTO
            {
                SatelliteId = satelliteId,
                Timestamp = latest.Timestamp,
                BatteryLevel = latest.BatteryLevel,
                Temperature = latest.Temperature
            },
            avgTemp);
    }

    public async Task<IReadOnlyList<AlertDTO>> GetActiveAlertsAsync(AlertFiltersDTO filters)
    {
        _logger.LogInformation("Fetching filtered alerts. Page: {Page}, Size: {Size}",
            filters.PageNumber, filters.PageSize);
        var alerts = await _satelliteTelemetryRepository.GetActiveFilteredAlertsAsync(filters);
        var alertDtos = alerts.Select(alertEntity => new AlertDTO(
            alertEntity.SatelliteId,
            alertEntity.Message,
            alertEntity.AlertType,
            alertEntity.CreatedAt
        )).ToList();

        _logger.LogInformation("Successfully retrieved {Count} alerts.", alertDtos.Count);

        return alertDtos.AsReadOnly();
    }
}
