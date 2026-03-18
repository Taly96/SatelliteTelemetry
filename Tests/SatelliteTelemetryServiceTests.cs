using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using SatelliteTelemetryAPI.API.DTOs;
using SatelliteTelemetryAPI.Core.Enums;
using SatelliteTelemetryAPI.Core.Interfaces;
using SatelliteTelemetryAPI.Core.Services;
using SatelliteTelemetryAPI.Infrastructure.Persistence.Entities;

namespace Tests;

public class SatelliteTelemetryServiceTests
{
    private readonly Mock<ILogger<SatelliteTelemetryService>> _loggerMock;
    private readonly Mock<ITelemetryRepository> _repositoryMock;
    private readonly SatelliteTelemetryService _service;

    public SatelliteTelemetryServiceTests()
    {
        _repositoryMock = new Mock<ITelemetryRepository>();
        _loggerMock = new Mock<ILogger<SatelliteTelemetryService>>();
        _service = new SatelliteTelemetryService(_repositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task AddTelemetries_ShouldCreateAlert_WhenBatteryIsLow()
    {
        var satelliteId = Guid.NewGuid();
        var records = new List<SatelliteTelemetryDTO>
        {
            new() { SatelliteId = satelliteId, Timestamp = DateTime.UtcNow, BatteryLevel = 15, Temperature = 25 }
        };

        await _service.AddTelemetriesToRecordAsync(records);
        _repositoryMock.Verify(r => r.AddAlertAsync(It.Is<AlertEntity>(a =>
            a.SatelliteId == satelliteId &&
            a.AlertType == AlertType.BatteryDrop)), Times.Once);
    }

    [Fact]
    public async Task GetSatelliteStatus_ShouldReturnNull_WhenNoReadingsExist()
    {
        var satelliteId = Guid.NewGuid();
        _repositoryMock.Setup(r => r.GetRecentTelemetryReadingsAsync(satelliteId, It.IsAny<int>()))
            .ReturnsAsync(new List<SatelliteTelemetryEntity>());
        var result = await _service.GetSatelliteStatusAsync(satelliteId);

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetSatelliteStatus_ShouldCalculateAverageTemperatureCorrectly()
    {
        var satelliteId = Guid.NewGuid();
        var readings = new List<SatelliteTelemetryEntity>
        {
            new(satelliteId, DateTime.UtcNow, 80, 40),
            new(satelliteId, DateTime.UtcNow.AddMinutes(-1), 80, 60)
        };

        _repositoryMock.Setup(r => r.GetRecentTelemetryReadingsAsync(satelliteId, 10))
            .ReturnsAsync(readings);
        var result = await _service.GetSatelliteStatusAsync(satelliteId);

        result.Should().NotBeNull();
        result.AverageTemperature.Should().Be(50);
    }

    [Fact]
    public async Task AddTelemetries_ShouldNotCreateAlert_WhenDataIsNormal()
    {
        var records = new List<SatelliteTelemetryDTO>
        {
            new() { SatelliteId = Guid.NewGuid(), Timestamp = DateTime.UtcNow, BatteryLevel = 50, Temperature = 30 }
        };

        await _service.AddTelemetriesToRecordAsync(records);
        _repositoryMock.Verify(r => r.AddAlertAsync(It.IsAny<AlertEntity>()), Times.Never);
    }
}