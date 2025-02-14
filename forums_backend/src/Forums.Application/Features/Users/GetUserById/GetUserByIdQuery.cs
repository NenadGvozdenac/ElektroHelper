using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure;
using MediatR;

namespace forums_backend.src.Forums.Application.Features.Users.GetUserById;

public record GetUserByIdQuery(UserDTO UserDTO, string Id) : IRequest<Result<UserByIdDTO>>;