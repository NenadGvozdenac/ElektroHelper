using forums_backend.src.Forums.Internal.API.DTOs.Forums;
using forums_backend.src.Forums.Internal.API.Public;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace forums_backend.src.Forums.API.Controllers.Forums;

[ApiController]
[Route("api/forums")]
public class ForumsController(IForumsService forumsService) : ControllerBase {
    private readonly IForumsService _forumsService = forumsService;

    [HttpGet]
    public async Task<ActionResult<List<ForumDTO>>> GetAllForumsAsync() {
        var forums = await _forumsService.GetForumsAsync();
        return Ok(forums);
    }
}