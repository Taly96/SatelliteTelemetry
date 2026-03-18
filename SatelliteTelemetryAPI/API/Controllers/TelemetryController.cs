using Microsoft.AspNetCore.Mvc;
using SatelliteTelemetryAPI.API.DTOs;
using SatelliteTelemetryAPI.Core.Interfaces;
using SatelliteTelemetryAPI.Core.Models;

namespace SatelliteTelemetryAPI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TelemetryController : ControllerBase
{
    private readonly ILogger<TelemetryController> _logger;

    private readonly ITelemetryService _telemetryService;

    public TelemetryController(ILogger<TelemetryController> logger, ITelemetryService telemetryService)
    {
        _logger = logger;
        _telemetryService = telemetryService;
    }

    [HttpPost]
    public async Task<IActionResult> AddSatellitesTelemetries([FromBody] List<SatelliteTelemetryDTO>? telemetryList)
    {
        if (telemetryList == null || telemetryList.Count == 0)
        {
            _logger.LogWarning("AddSatellitesTelemetries called with null or empty list.");

            return BadRequest("Telemetry list cannot be null or empty.");
        }

        try
        {
            _logger.LogInformation("Processing batch of {Count} telemetry records.", telemetryList.Count);
            await _telemetryService.AddTelemetriesToRecordAsync(telemetryList);

            return StatusCode(StatusCodes.Status201Created, new { message = "Records added successfully" });
        }
        catch (ArgumentException argumentException)
        {
            _logger.LogWarning(argumentException, "Validation error while processing telemetry.");

            return BadRequest(argumentException.Message);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Unexpected error in AddSatellitesTelemetries.");

            return StatusCode(StatusCodes.Status500InternalServerError,
                "An internal error occurred while processing telemetry.");
        }
    }

    [HttpGet("satellites/{id}/status")]
    public async Task<ActionResult<SatelliteStatusDTO>> GetSatelliteStatus(Guid satelliteId)
    {
        if (satelliteId == Guid.Empty)
        {
            _logger.LogWarning("GetStatus called with an empty Guid.");

            return BadRequest("A valid Satellite ID is required.");
        }

        try
        {
            _logger.LogDebug("Fetching status for satellite: {SatelliteId}", satelliteId);
            var satelliteStatus = await _telemetryService.GetSatelliteStatusAsync(satelliteId);

            if (satelliteStatus != null)
            {
                _logger.LogInformation("Found satellite status: {SatelliteId}", satelliteId);

                return Ok(satelliteStatus);
            }

            _logger.LogInformation("Status not found for satellite: {SatelliteId}", satelliteId);

            return NotFound($"Satellite {satelliteId} not found.");
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error fetching status for satellite {SatelliteId}", satelliteId);

            return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving satellite status.");
        }
    }

    [HttpGet("alerts")]
    public async Task<ActionResult<IEnumerable<AlertDTO>>> GetSatellitesAlerts(
        [FromQuery] AlertFiltersDTO alertsFilters)
    {
        try
        {
            _logger.LogInformation("Fetching alerts: Page {Page}, Size {Size}", alertsFilters.PageNumber,
                alertsFilters.PageSize);

            var activeAlerts = await _telemetryService.GetActiveAlertsAsync(alertsFilters);

            return Ok(activeAlerts);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error fetching alerts with provided filters.");

            return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving alerts.");
        }
    }
}