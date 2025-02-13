using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure;
using MediatR;

namespace forums_backend.src.Forums.Application.Features.Posts.GetAllPosts;

public record GetAllPostsQuery(UserDTO UserDTO) : IRequest<Result<List<PostDTO>>>;