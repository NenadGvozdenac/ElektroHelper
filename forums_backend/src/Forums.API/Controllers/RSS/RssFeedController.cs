using forums_backend.src.Forums.Application.Features.RSS.GetRssFeed;
using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace forums_backend.src.Forums.API.Controllers.RSS;

[ApiController]
[Route("api/rss")]
public class RssFeedController(IMediator mediator) : BaseController
{
    [HttpGet]
    public async Task<ActionResult<Result>> GetRssFeed()
    {
        var response = await mediator.Send(new GetRssFeedQuery());
        return CreateResponse(response);
    }
}