using forums_backend.src.Forums.API.DTOs;
using forums_backend.src.Forums.Application.Features.Comments.CreateComment;
using forums_backend.src.Forums.Application.Features.Comments.GetCommentsForPost;
using forums_backend.src.Forums.Application.Features.Comments.GetMyComments;
using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace forums_backend.src.Forums.API.Controllers.Comments;

[ApiController]
[Route("api/comments")]
[Authorize]
public class CommentsController(IMediator mediator) : BaseController {

    [HttpPost]
    public async Task<ActionResult<Result>> CreateCommentAsync(CreateCommentDTO createCommentDTO) {
        var comment = await mediator.Send(new CreateCommentCommand(this.GetUser(), createCommentDTO.Content, createCommentDTO.PostId));
        return CreateResponse(comment);
    }

    [HttpGet("{postId}")]
    public async Task<ActionResult<Result>> GetCommentsForPost(Guid postId) {
        var comments = await mediator.Send(new GetCommentsForPostQuery(this.GetUser(), postId));
        return CreateResponse(comments);
    }

    [HttpGet("my")]
    public async Task<ActionResult<Result>> GetMyCommentsAsync() {
        var comments = await mediator.Send(new GetMyCommentsQuery(this.GetUser()));
        return CreateResponse(comments);
    }
}