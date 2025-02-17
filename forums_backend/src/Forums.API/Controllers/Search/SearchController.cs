using forums_backend.src.Forums.Application.BackgroundServices;
using forums_backend.src.Forums.Application.Features.Search.SearchPosts;
using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace forums_backend.src.Forums.API.Controllers.Search;


[ApiController]
[Route("api/search")]
[Authorize]
public class SearchController(IMediator mediator, PostSyncService postSyncService) : BaseController
{
    [HttpGet("search")]
    public async Task<ActionResult<Result>> SearchPostsAsync([FromQuery] string query, [FromQuery] int page, [FromQuery] int pageSize)
    {
        var posts = await mediator.Send(new SearchPostsQuery(query, page, pageSize));
        return CreateResponse(posts);
    }

    [HttpPost("refresh")]
    public async Task<ActionResult> RefreshSearchIndexAsync()
    {
        await postSyncService.ManuallySyncPostsAsync(CancellationToken.None);
        return Ok();
    }
}