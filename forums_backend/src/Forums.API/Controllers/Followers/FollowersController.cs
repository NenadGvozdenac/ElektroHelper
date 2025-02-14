using forums_backend.src.Forums.Application.Features.Followers.FollowUser;
using forums_backend.src.Forums.Application.Features.Followers.GetMyFollowers;
using forums_backend.src.Forums.Application.Features.Followers.GetMyFollowersPaged;
using forums_backend.src.Forums.Application.Features.Followers.GetPostsByFollowers;
using forums_backend.src.Forums.Application.Features.Followers.GetPostsByFollowersPaged;
using forums_backend.src.Forums.Application.Features.Followers.UnfollowUser;
using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace forums_backend.src.Forums.API.Controllers.Followers;

[ApiController]
[Route("api/followers")]
[Authorize]
public class FollowersController(IMediator mediator) : BaseController {
    
    [HttpPost("follow/{followerId}")]
    public async Task<Result> FollowUser(string followerId) {
        var result = await mediator.Send(new FollowUserCommand(this.GetUser(), followerId));
        return result;
    }

    [HttpPost("unfollow/{followerId}")]
    public async Task<Result> UnfollowUser(string followerId) {
        var result = await mediator.Send(new UnfollowUserCommand(this.GetUser(), followerId));
        return result;
    }

    [HttpGet("my")]
    public async Task<Result> GetMyFollowers(int? page, int? pageSize) {
        if(page.HasValue && pageSize.HasValue) {
            var result = await mediator.Send(new GetMyFollowersPagedQuery(this.GetUser(), page.Value, pageSize.Value));
            return result;
        } else {
            var result = await mediator.Send(new GetMyFollowersQuery(this.GetUser()));
            return result;
        }
    }

    [HttpGet("posts")]
    public async Task<Result> GetPostsByFollowers(int? page, int? pageSize) {
        if(page.HasValue && pageSize.HasValue) {
            var result = await mediator.Send(new GetPostsByFollowersPagedQuery(this.GetUser(), page.Value, pageSize.Value));
            return result;
        } else {
            var result = await mediator.Send(new GetPostsByFollowersQuery(this.GetUser()));
            return result;
        }
    }
}