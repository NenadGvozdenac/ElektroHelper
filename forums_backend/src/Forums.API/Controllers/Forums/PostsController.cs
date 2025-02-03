using forums_backend.src.Forums.Internal.API.DTOs.Forums;
using forums_backend.src.Forums.Internal.API.DTOs.Posts;
using forums_backend.src.Forums.Internal.API.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace forums_backend.src.Forums.API.Controllers.Forums;

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
        var post = await _forumsService.CreatePostAsync(createPostDTO);
        return Ok(post);
    }

    [HttpGet("{forumId}")]
    public async Task<ActionResult<List<PostDTO>>> GetPostsByForumIdAsync(Guid forumId) {
        var posts = await _forumsService.GetPostsByForumIdAsync(forumId);
        return Ok(posts);
    }
}