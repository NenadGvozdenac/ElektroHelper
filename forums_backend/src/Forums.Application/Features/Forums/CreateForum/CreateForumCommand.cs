using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure;
using MediatR;

namespace forums_backend.src.Forums.Application.Features.Forums.CreateForum;

public record CreateForumCommand(UserDTO UserDTO, string Name, string Description) : IRequest<Result<CreatedForumDTO>>;