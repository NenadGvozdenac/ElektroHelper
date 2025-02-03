using forums_backend.src.Forums.BuildingBlocks.Infrastructure;
using forums_backend.src.Forums.Internal.API.DTOs.Forums;
using forums_backend.src.Forums.Internal.API.Public;
using forums_backend.src.Forums.Internal.Core.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace forums_backend.src.Forums.API.Controllers.Forums;

[Route("api/forums")]
[ApiController]
[Authorize]
public class ForumsController : ControllerBase {

    private readonly IForumsService _forumsService;
    public ForumsController(IForumsService forumsService)
    {
        _forumsService = forumsService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ForumDTO>>> GetAllForumsAsync() {
        var forums = await _forumsService.GetForumsAsync();
        return Ok(forums);
    }

    [HttpPost]
    public async Task<ActionResult<ForumDTO>> CreateForumAsync(CreateForumDTO createForumDTO) {
        var forum = await _forumsService.CreateForumAsync(createForumDTO, this.GetUser());
        return Ok(forum);
    }
}