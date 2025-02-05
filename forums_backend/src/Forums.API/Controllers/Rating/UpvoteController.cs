using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure;
using forums_backend.src.Forums.Internal.API.DTOs.Rating.Comments;
using forums_backend.src.Forums.Internal.API.DTOs.Rating.Posts;
using forums_backend.src.Forums.Internal.API.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace forums_backend.src.Forums.API.Controllers.Rating;

[ApiController]
[Route("api/rating/upvote")]
[Authorize]
public class UpvoteController : BaseController
{
    private readonly IUpvoteService _upvoteService;

    public UpvoteController(IUpvoteService upvoteService)
    {
        _upvoteService = upvoteService;
    }

    [HttpPost("comment")]
    public async Task<ActionResult<Result<UpvoteCommentDTO>>> UpvoteComment([FromQuery] Guid commentId)
    {
        var result = await _upvoteService.UpvoteCommentAsync(commentId, this.GetUser());
        return CreateResponse(result);
    }

    [HttpPost("post")]
    public async Task<ActionResult<Result<UpvotePostDTO>>> UpvotePost([FromQuery] Guid postId)
    {
        var result = await _upvoteService.UpvotePostAsync(postId, this.GetUser());
        return CreateResponse(result);
    }

    [HttpDelete("comment")]
    public async Task<ActionResult<Result<UpvoteCommentDTO>>> RemoveUpvoteFromComment([FromQuery] Guid commentId)
    {
        var result = await _upvoteService.RemoveUpvoteFromCommentAsync(commentId, this.GetUser());
        return CreateResponse(result);
    }

    [HttpDelete("post")]
    public async Task<ActionResult<Result<UpvotePostDTO>>> RemoveUpvoteFromPost([FromQuery] Guid postId)
    {
        var result = await _upvoteService.RemoveUpvoteFromPostAsync(postId, this.GetUser());
        return CreateResponse(result);
    }
}