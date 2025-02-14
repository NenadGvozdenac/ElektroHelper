using forums_backend.src.Forums.Application.Features.Users.CreateUser;
using forums_backend.src.Forums.Application.Features.Users.GetUserById;
using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace forums_backend.src.Forums.API.Controllers.Users;

[ApiController]
[Route("api/users")]
[Authorize]
public class UserController(IMediator mediator) : BaseController {

    [HttpPost]
    public async Task<ActionResult> CreateUser() {
        var user = this.GetUser();
        var response = await mediator.Send(new CreateUserCommand(new CreateUserDTO(user.Id, user.Username, user.Email, user.Role)));
        return CreateResponse(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Result>> GetUserById(string id) {
        var response = await mediator.Send(new GetUserByIdQuery(this.GetUser(), id));
        return CreateResponse(response);
    }

    [HttpGet("my")]
    public async Task<ActionResult<Result>> GetMyUser() {
        var response = await mediator.Send(new GetUserByIdQuery(this.GetUser(), this.GetUser().Id));
        return CreateResponse(response);
    }
}