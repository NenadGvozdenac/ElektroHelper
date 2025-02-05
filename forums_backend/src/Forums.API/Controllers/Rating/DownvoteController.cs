using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace forums_backend.src.Forums.API.Controllers.Rating;

[ApiController]
[Route("api/rating/downvote")]
[Authorize]
public class DownvoteController : BaseController {
    
}