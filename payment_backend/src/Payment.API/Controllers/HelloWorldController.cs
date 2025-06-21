using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using payment_backend.src.Payment.BuildingBlocks.Core.Domain;
using payment_backend.src.Payment.BuildingBlocks.Infrastructure;

namespace payment_backend.src.Payment.API.Controllers;

[ApiController]
[Route("api/payments")]
public class HelloWorldController(IMediator mediator) : BaseController {
    [HttpGet("hello-world")]
    public ActionResult HelloWorld() {
        return Ok("Hello World!");
    }
}