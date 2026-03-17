using Microsoft.AspNetCore.Mvc;
using SatelliteTelemetryAPI.API.DTOs;
using SatelliteTelemetryAPI.Core.Interfaces;

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
    public async Task<IActionResult> AddSatellitesTelemetries([FromBody] List<TelemetryDTO>? telemetryList)
    {
        if (telemetryList == null || telemetryList.Count == 0)
            return BadRequest("Telemetry list cannot be null or empty.");

        try
        {
            var success = await _telemetryService.AddTelemetriesToRecordAsync(telemetryList);

            if (success)
            {
                _logger.LogInformation("Successfully processed {Count} telemetry records.", telemetryList.Count);

                return StatusCode(StatusCodes.Status201Created, new { message = "Records added successfully" });
            }

            _logger.LogWarning("Telemetry service returned failure for batch update.");

            return UnprocessableEntity("The data was valid but could not be processed at this time.");
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "An unhandled error occurred while adding telemetries.");

            return StatusCode(StatusCodes.Status500InternalServerError, "An internal error occurred.");
        }
    }

    [HttpGet(":id/status")]
    public IActionResult GetSatelliteStatus()
    {
        return Ok();
    }

    [HttpGet("alerts")]
    public IActionResult GetActiveAlerts()
    {
        return Ok();
    }
}