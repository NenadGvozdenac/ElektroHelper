using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure;
using forums_backend.src.Forums.Internal.API.DTOs.Forums;
using forums_backend.src.Forums.Internal.API.DTOs.Posts;
using forums_backend.src.Forums.Internal.API.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace forums_backend.src.Forums.API.Controllers.Posts;

[ApiController]
[Route("api/posts")]
[Authorize]
public class PostsController : BaseController
{
    private readonly IPostsService _forumsService;

    public PostsController(IPostsService forumsService)
    {
        _forumsService = forumsService;
    }

    [HttpGet]
    public async Task<ActionResult<Result<List<PostDTO>>>> GetAllPostsAsync([FromQuery] int? page = null, [FromQuery] int? pageSize = null)
    {
        var posts = page.HasValue && pageSize.HasValue
            ? await _forumsService.GetPostsAsync(page.Value, pageSize.Value, this.GetUser())
            : await _forumsService.GetPostsAsync();

        return CreateResponse(posts);
    }

    [HttpPost]
    public async Task<ActionResult<Result<PostDTO>>> CreatePostAsync(CreatePostDTO createPostDTO)
    {
        var post = await _forumsService.CreatePostAsync(createPostDTO, this.GetUser());
        return CreateResponse(post);
    }

    [HttpGet("{forumId}")]
    public async Task<ActionResult<Result<List<PostDTO>>>> GetPostsByForumIdAsync(Guid forumId, [FromQuery] int? page = null, [FromQuery] int? pageSize = null)
    {
        var posts = page.HasValue && pageSize.HasValue
            ? await _forumsService.GetPostsByForumIdPagedAsync(page.Value, pageSize.Value, forumId, this.GetUser())
            : await _forumsService.GetPostsByForumIdAsync(forumId, this.GetUser());
        return CreateResponse(posts);
    }

    [HttpGet("my")]
    public async Task<ActionResult<Result<List<PostDTO>>>> GetMyPostsAsync()
    {
        var posts = await _forumsService.GetMyPostsAsync(this.GetUser());
        return CreateResponse(posts);
    }
}