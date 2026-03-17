using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace SatelliteTelemetryAPI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TelemetryController : ControllerBase
{
    [HttpPost]
    public IActionResult AddSatelliteTelemetry()
    {
        return Ok();
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