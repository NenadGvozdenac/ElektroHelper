using forums_backend.src.Forums.Application.Features.Comments.DownvoteComment;
using forums_backend.src.Forums.Application.Features.Comments.RemoveCommentDownvote;
using forums_backend.src.Forums.Application.Features.Posts.DownvotePost;
using forums_backend.src.Forums.Application.Features.Posts.RemovePostDownvote;
using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace forums_backend.src.Forums.API.Controllers.Rating;

[ApiController]
[Route("api/rating/downvote")]
[Authorize]
public class DownvoteController(IMediator mediator) : BaseController {

    [HttpPost("comment")]
    public async Task<ActionResult<Result>> DownvoteComment([FromQuery] Guid commentId)
    {
        var result = await mediator.Send(new DownvoteCommentCommand(this.GetUser(), commentId));
        return CreateResponse(result);
    }

    [HttpPost("post")]
    public async Task<ActionResult<Result>> DownvotePost([FromQuery] Guid postId)
    {
        var result = await mediator.Send(new DownvotePostCommand(this.GetUser(), postId));
        return CreateResponse(result);
    }

    [HttpDelete("comment")]
    public async Task<ActionResult<Result>> RemoveDownvoteFromComment([FromQuery] Guid commentId)
    {
        var result = await mediator.Send(new RemoveCommentDownvoteCommand(this.GetUser(), commentId));
        return CreateResponse(result);
    }

    [HttpDelete("post")]
    public async Task<ActionResult<Result>> RemoveDownvoteFromPost([FromQuery] Guid postId)
    {
        var result = await mediator.Send(new RemovePostDownvoteCommand(this.GetUser(), postId));
        return CreateResponse(result);
    }
}