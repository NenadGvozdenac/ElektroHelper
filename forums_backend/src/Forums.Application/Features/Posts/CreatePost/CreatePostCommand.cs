using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure;
using MediatR;

namespace forums_backend.src.Forums.Application.Features.Posts.CreatePost;

public record CreatePostCommand(CreatePostDTO CreatePostDTO, UserDTO UserDTO) : IRequest<Result<CreatedPostDTO>>;