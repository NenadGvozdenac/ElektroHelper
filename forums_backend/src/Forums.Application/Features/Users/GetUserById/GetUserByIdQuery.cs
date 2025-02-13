using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using MediatR;

namespace forums_backend.src.Forums.Application.Features.Users.GetUserById;

public record GetUserByIdQuery(string Id) : IRequest<Result<UserByIdDTO>>;