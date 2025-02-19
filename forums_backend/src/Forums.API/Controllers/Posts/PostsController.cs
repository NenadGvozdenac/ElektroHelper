using forums_backend.src.Forums.Application.Features.Posts.CreatePost;
using forums_backend.src.Forums.Application.Features.Posts.DeletePost;
using forums_backend.src.Forums.Application.Features.Posts.DeletePostByAdmin;
using forums_backend.src.Forums.Application.Features.Posts.GetAllPosts;
using forums_backend.src.Forums.Application.Features.Posts.GetAllPostsPaged;
using forums_backend.src.Forums.Application.Features.Posts.GetMyPosts;
using forums_backend.src.Forums.Application.Features.Posts.GetPostById;
using forums_backend.src.Forums.Application.Features.Posts.GetPostsByForumId;
using forums_backend.src.Forums.Application.Features.Posts.GetPostsByForumIdPaged;
using forums_backend.src.Forums.Application.Features.Posts.GetPostsByUserId;
using forums_backend.src.Forums.Application.Features.Posts.GetPostsByUserIdPaged;
using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace forums_backend.src.Forums.API.Controllers.Posts;

[ApiController]
[Route("api/posts")]
[Authorize]
public class PostsController(IMediator mediator) : BaseController
{
    [HttpGet]
    public async Task<ActionResult<Result>> GetAllPostsAsync([FromQuery] int? page = null, [FromQuery] int? pageSize = null)
    {
        if(page.HasValue && pageSize.HasValue)
        {
            var posts = await mediator.Send(new GetAllPostsPagedQuery(this.GetUser(), page.Value, pageSize.Value));
            return CreateResponse(posts);
        }
        else
        {
            var posts = await mediator.Send(new GetAllPostsQuery(this.GetUser()));
            return CreateResponse(posts);
        }
    }

    [HttpPost]
    public async Task<ActionResult<Result>> CreatePostAsync(CreatePostDTO createPostDTO)
    {
        var post = await mediator.Send(new CreatePostCommand(createPostDTO, this.GetUser()));
        return CreateResponse(post);
    }

    [HttpGet("forum/{forumId}")]
    public async Task<ActionResult<Result>> GetPostsByForumIdAsync(Guid forumId, [FromQuery] int? page = null, [FromQuery] int? pageSize = null)
    {
        if(page.HasValue && pageSize.HasValue)
        {
            var posts = await mediator.Send(new GetPostsByForumIdPagedQuery(this.GetUser(), forumId, page.Value, pageSize.Value));
            return CreateResponse(posts);
        }
        else
        {
            var posts = await mediator.Send(new GetPostsByForumIdQuery(this.GetUser(), forumId));
            return CreateResponse(posts);
        }
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<Result>> GetPostsByUserIdAsync(string userId, [FromQuery] int? page = null, [FromQuery] int? pageSize = null)
    {
        if(page.HasValue && pageSize.HasValue)
        {
            var posts = await mediator.Send(new GetPostsByUserIdPagedQuery(this.GetUser(), userId, page.Value, pageSize.Value));
            return CreateResponse(posts);
        }
        else
        {
            var posts = await mediator.Send(new GetPostsByUserIdQuery(this.GetUser(), userId));
            return CreateResponse(posts);
        }
    }

    [HttpGet("my")]
    public async Task<ActionResult<Result>> GetMyPostsAsync()
    {
        var posts = await mediator.Send(new GetMyPostsQuery(this.GetUser()));
        return CreateResponse(posts);
    }

    [HttpGet("{postId}")]
    public async Task<ActionResult<Result>> GetPostByIdAsync(Guid postId)
    {
        var post = await mediator.Send(new GetPostByIdQuery(this.GetUser(), postId));
        return CreateResponse(post);
    }

    [HttpDelete("{postId}")]
    public async Task<ActionResult<Result>> DeletePostAsync(Guid postId)
    {
        var post = await mediator.Send(new DeletePostCommand(this.GetUser(), postId));
        return CreateResponse(post);
    }

    [HttpDelete("admin/{postId}")]
    [Authorize(Policy = "adminPolicy")]
    public async Task<ActionResult<Result>> DeletePostByAdminAsync(Guid postId)
    {
        var post = await mediator.Send(new DeletePostByAdminCommand(this.GetUser(), postId));
        return CreateResponse(post);
    }
}