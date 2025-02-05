using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure;
using forums_backend.src.Forums.Internal.API.DTOs.Forums;
using forums_backend.src.Forums.Internal.API.Public;
using forums_backend.src.Forums.Internal.Core.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace forums_backend.src.Forums.API.Controllers.Forums;

[ApiController]
[Route("api/forums")]
[Authorize]
public class ForumsController : BaseController {

    private readonly IForumsService _forumsService;
    public ForumsController(IForumsService forumsService)
    {
        _forumsService = forumsService;
    }

    [HttpGet]
    public async Task<ActionResult<Result<List<ForumDTO>>>> GetAllForumsAsync() {
        var forums = await _forumsService.GetForumsAsync();
        return CreateResponse(forums);
    }

    [HttpPost]
    public async Task<ActionResult<Result<ForumDTO>>> CreateForumAsync(CreateForumDTO createForumDTO) {
        var forum = await _forumsService.CreateForumAsync(createForumDTO, this.GetUser());
        return CreateResponse(forum);
    }
}