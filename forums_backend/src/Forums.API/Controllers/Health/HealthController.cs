using Microsoft.AspNetCore.Mvc;
using forums_backend.src.Forums.BuildingBlocks.Core.Domain;

namespace forums_backend.src.Forums.API.Controllers.Health;

[ApiController]
[Route("api/health")]
public class HealthController : BaseController
{
    [HttpGet]
    public IActionResult GetHealth()
    {
        var healthStatus = new
        {
            status = "healthy",
            service = "elektrohelper-forums-api",
            timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
        };

        return Ok(healthStatus);
    }
}
