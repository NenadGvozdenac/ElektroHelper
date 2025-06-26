using Microsoft.AspNetCore.Mvc;
using payment_backend.src.Payment.BuildingBlocks.Core.Domain;

namespace payment_backend.src.Payment.API.Controllers.Health;

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
            service = "elektrohelper-payment-api",
            timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
        };

        return Ok(healthStatus);
    }
}
