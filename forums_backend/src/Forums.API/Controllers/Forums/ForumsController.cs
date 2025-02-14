using forums_backend.src.Forums.API.DTOs;
using forums_backend.src.Forums.Application.Features.Forums.CreateForum;
using forums_backend.src.Forums.Application.Features.Forums.GetAllForums;
using forums_backend.src.Forums.Application.Features.Forums.GetForumById;
using forums_backend.src.Forums.Application.Features.Forums.QuarantineForum;
using forums_backend.src.Forums.Application.Features.Forums.UnquarantineForum;
using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace forums_backend.src.Forums.API.Controllers.Forums;

[ApiController]
[Route("api/forums")]
[Authorize]
public class ForumsController(IMediator mediator) : BaseController {

    [HttpGet]
    public async Task<ActionResult<Result>> GetAllForumsAsync() {
        var forums = await mediator.Send(new GetAllForumsQuery());
        return CreateResponse(forums);
    }

    [HttpGet]
    [Route("{forumId}")]
    public async Task<ActionResult<Result>> GetForumAsync(Guid forumId) {
        var forum = await mediator.Send(new GetForumByIdQuery(forumId));
        return CreateResponse(forum);
    }

    [HttpPost]
    public async Task<ActionResult<Result>> CreateForumAsync(CreateForumDTO createForumDTO) {
        var forum = await mediator.Send(new CreateForumCommand(this.GetUser(), createForumDTO.Name, createForumDTO.Description));
        return CreateResponse(forum);
    }

    [HttpPost("quarantine/{forumId}")]
    public async Task<ActionResult<Result>> QuarantineForumAsync(Guid forumId) {
        var forum = await mediator.Send(new QuarantineForumCommand(this.GetUser(), forumId));
        return CreateResponse(forum);
    }

    [HttpPost("unquarantine/{forumId}")]
    public async Task<ActionResult<Result>> UnquarantineForumAsync(Guid forumId) {
        var forum = await mediator.Send(new UnquarantineForumCommand(this.GetUser(), forumId));
        return CreateResponse(forum);
    }
}