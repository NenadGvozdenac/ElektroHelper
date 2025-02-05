
using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure;
using forums_backend.src.Forums.Internal.API.DTOs.Comments;
using forums_backend.src.Forums.Internal.API.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace forums_backend.src.Forums.API.Controllers.Comments;

[ApiController]
[Route("api/comments")]
[Authorize]
public class CommentsController : BaseController {
    private readonly ICommentsService _commentsService;

    public CommentsController(ICommentsService commentsService) {
        _commentsService = commentsService;
    }

    [HttpPost]
    public async Task<ActionResult<Result<CommentDTO>>> CreateCommentAsync(CreateCommentDTO createCommentDTO) {
        var comment = await _commentsService.CreateCommentAsync(createCommentDTO, this.GetUser());
        return CreateResponse(comment);
    }

    [HttpGet("{postId}")]
    public async Task<ActionResult<Result<List<CommentDTO>>>> GetPostAndItsCommentsAsync(Guid postId) {
        var comments = await _commentsService.GetPostAndItsCommentsAsync(postId);
        return CreateResponse(comments);
    }

    [HttpGet("my")]
    public async Task<ActionResult<Result<List<CommentDTO>>>> GetMyCommentsAsync() {
        var comments = await _commentsService.GetMyCommentsAsync(this.GetUser());
        return CreateResponse(comments);
    }
}