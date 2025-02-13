using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure;
using MediatR;

namespace forums_backend.src.Forums.Application.Features.Posts.GetPostsByForumId;

public record GetPostsByForumIdQuery(UserDTO UserDTO, Guid ForumId) : IRequest<Result<List<PostDTO>>>;