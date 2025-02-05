using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure;
using forums_backend.src.Forums.Internal.API.DTOs.Rating.Comments;
using forums_backend.src.Forums.Internal.API.DTOs.Rating.Posts;
using forums_backend.src.Forums.Internal.API.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace forums_backend.src.Forums.API.Controllers.Rating;

[ApiController]
[Route("api/rating/downvote")]
[Authorize]
public class DownvoteController : BaseController {
    private readonly IDownvoteService _downvoteService;

    public DownvoteController(IDownvoteService downvoteService)
    {
        _downvoteService = downvoteService;
    }

    [HttpPost("comment")]
    public async Task<ActionResult<Result<DownvoteCommentDTO>>> DownvoteComment([FromQuery] Guid commentId)
    {
        var result = await _downvoteService.DownvoteCommentAsync(commentId, this.GetUser());
        return CreateResponse(result);
    }

    [HttpPost("post")]
    public async Task<ActionResult<Result<DownvotePostDTO>>> DownvotePost([FromQuery] Guid postId)
    {
        var result = await _downvoteService.DownvotePostAsync(postId, this.GetUser());
        return CreateResponse(result);
    }

    [HttpDelete("comment")]
    public async Task<ActionResult<Result<DownvoteCommentDTO>>> RemoveDownvoteFromComment([FromQuery] Guid commentId)
    {
        var result = await _downvoteService.RemoveDownvoteFromCommentAsync(commentId, this.GetUser());
        return CreateResponse(result);
    }

    [HttpDelete("post")]
    public async Task<ActionResult<Result<DownvotePostDTO>>> RemoveDownvoteFromPost([FromQuery] Guid postId)
    {
        var result = await _downvoteService.RemoveDownvoteFromPostAsync(postId, this.GetUser());
        return CreateResponse(result);
    }
}