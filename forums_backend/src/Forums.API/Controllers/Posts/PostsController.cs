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
public class PostsController: ControllerBase {
    private readonly IPostsService _forumsService;

    public PostsController(IPostsService forumsService) {
        _forumsService = forumsService;
    }

    [HttpGet]
    public async Task<ActionResult<List<PostDTO>>> GetAllPostsAsync() {
        var posts = await _forumsService.GetPostsAsync();
        return Ok(posts);
    }

    [HttpPost]
    public async Task<ActionResult<PostDTO>> CreatePostAsync(CreatePostDTO createPostDTO) {
        var post = await _forumsService.CreatePostAsync(createPostDTO, this.GetUser());
        return Ok(post);
    }

    [HttpGet("{forumId}")]
    public async Task<ActionResult<List<PostDTO>>> GetPostsByForumIdAsync(Guid forumId) {
        var posts = await _forumsService.GetPostsByForumIdAsync(forumId);
        return Ok(posts);
    }

    [HttpGet("my")]
    public async Task<ActionResult<List<PostDTO>>> GetMyPostsAsync() {
        var posts = await _forumsService.GetMyPostsAsync(this.GetUser());
        return Ok(posts);
    }
}