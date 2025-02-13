using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using MediatR;

namespace forums_backend.src.Forums.Application.Features.Users.CreateUser;

public record CreateUserCommand(CreateUserDTO CreateUserDTO) : IRequest<Result<CreatedUserDTO>>;