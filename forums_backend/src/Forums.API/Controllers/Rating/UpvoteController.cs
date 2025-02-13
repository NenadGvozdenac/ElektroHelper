using forums_backend.src.Forums.Application.Features.Comments.RemoveCommentUpvote;
using forums_backend.src.Forums.Application.Features.Comments.UpvoteComment;
using forums_backend.src.Forums.Application.Features.Posts.RemovePostUpvote;
using forums_backend.src.Forums.Application.Features.Posts.UpvotePost;
using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace forums_backend.src.Forums.API.Controllers.Rating;

[ApiController]
[Route("api/rating/upvote")]
[Authorize]
public class UpvoteController(IMediator mediator) : BaseController
{

    [HttpPost("comment")]
    public async Task<ActionResult<Result>> UpvoteComment([FromQuery] Guid commentId)
    {
        var result = await mediator.Send(new UpvoteCommentCommand(this.GetUser(), commentId));
        return CreateResponse(result);
    }

    [HttpPost("post")]
    public async Task<ActionResult<Result>> UpvotePost([FromQuery] Guid postId)
    {
        var result = await mediator.Send(new UpvotePostCommand(this.GetUser(), postId));
        return CreateResponse(result);
    }

    [HttpDelete("comment")]
    public async Task<ActionResult<Result>> RemoveUpvoteFromComment([FromQuery] Guid commentId)
    {
        var result = await mediator.Send(new RemoveCommentUpvoteCommand(this.GetUser(), commentId));
        return CreateResponse(result);
    }

    [HttpDelete("post")]
    public async Task<ActionResult<Result>> RemoveUpvoteFromPost([FromQuery] Guid postId)
    {
        var result = await mediator.Send(new RemovePostUpvoteCommand(this.GetUser(), postId));
        return CreateResponse(result);
    }
}