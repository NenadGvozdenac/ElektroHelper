using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using MediatR;

namespace forums_backend.src.Forums.Application.Features.Forums.GetForumById;

public record GetForumByIdQuery(Guid Id) : IRequest<Result<ForumDTO>>;