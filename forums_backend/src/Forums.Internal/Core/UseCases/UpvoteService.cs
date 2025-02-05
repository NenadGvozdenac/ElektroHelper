using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.Internal.API.DTOs.Rating.Comments;
using forums_backend.src.Forums.Internal.API.DTOs.Rating.Posts;
using forums_backend.src.Forums.Internal.API.DTOs.Users;
using forums_backend.src.Forums.Internal.API.Public;
using forums_backend.src.Forums.Internal.Core.Domain.RepositoryInterfaces;

namespace forums_backend.src.Forums.Internal.Core.UseCases;

public class UpvoteService : IUpvoteService
{
    private readonly IUpvotePostRepository _upvotePostRepository;
    private readonly IUpvoteCommentRepository _upvoteCommentRepository;

    public UpvoteService(IUpvotePostRepository upvotePostRepository, IUpvoteCommentRepository upvoteCommentRepository)
    {
        _upvotePostRepository = upvotePostRepository;
        _upvoteCommentRepository = upvoteCommentRepository;
    }

    public Task<Result<UpvoteCommentDTO>> RemoveUpvoteFromCommentAsync(Guid commentId, UserDTO user)
    {
        throw new NotImplementedException();
    }

    public Task<Result<UpvotePostDTO>> RemoveUpvoteFromPostAsync(Guid postId, UserDTO user)
    {
        throw new NotImplementedException();
    }

    public Task<Result<UpvoteCommentDTO>> UpvoteCommentAsync(Guid commentId, UserDTO user)
    {
        throw new NotImplementedException();
    }

    public Task<Result<UpvotePostDTO>> UpvotePostAsync(Guid postId, UserDTO user)
    {
        throw new NotImplementedException();
    }
}