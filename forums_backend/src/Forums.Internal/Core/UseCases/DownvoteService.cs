using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.Internal.API.DTOs.Rating.Comments;
using forums_backend.src.Forums.Internal.API.DTOs.Rating.Posts;
using forums_backend.src.Forums.Internal.API.DTOs.Users;
using forums_backend.src.Forums.Internal.API.Public;
using forums_backend.src.Forums.Internal.Core.Domain;
using forums_backend.src.Forums.Internal.Core.Domain.RepositoryInterfaces;

namespace forums_backend.src.Forums.Internal.Core.UseCases;

public class DownvoteService : IDownvoteService
{
    private readonly IDownvotePostRepository _downvotePostRepository;
    private readonly IDownvoteCommentRepository _downvoteCommentRepository;

    public DownvoteService(IDownvoteCommentRepository downvoteCommentRepository, IDownvotePostRepository downvotePostRepository)
    {
        _downvoteCommentRepository = downvoteCommentRepository;
        _downvotePostRepository = downvotePostRepository;
    }

    public async Task<Result<DownvoteCommentDTO>> DownvoteCommentAsync(Guid commentId, UserDTO userDTO)
    {
        User user = new(userDTO.Id, userDTO.Email, userDTO.Role, userDTO.Username);

        var userDownvotedComment = await _downvoteCommentRepository.UserDownvotedCommentAsync(commentId, user.Id);

        if (userDownvotedComment)
        {
            return Result<DownvoteCommentDTO>.Failure("User already downvoted this comment").WithCode(400);
        }

        // TODO:    Check if user upvoted the comment before downvoting it
        //          If user upvoted the comment, remove the upvote before adding the downvote

        var downvoteAdded = await _downvoteCommentRepository.AddDownvoteToCommentAsync(commentId, user.Id);

        if (!downvoteAdded)
        {
            return Result<DownvoteCommentDTO>.Failure("Comment not found").WithCode(404);
        }

        return Result<DownvoteCommentDTO>.Success(new DownvoteCommentDTO(commentId, true));
    }

    public async Task<Result<DownvoteCommentDTO>> RemoveDownvoteFromCommentAsync(Guid commentId, UserDTO userDTO)
    {
        User user = new(userDTO.Id, userDTO.Email, userDTO.Role, userDTO.Username);

        var userDownvotedComment = await _downvoteCommentRepository.UserDownvotedCommentAsync(commentId, user.Id);

        if (!userDownvotedComment)
        {
            return Result<DownvoteCommentDTO>.Failure("User did not downvote this comment").WithCode(400);
        }

        var removedDownvote = await _downvoteCommentRepository.RemoveDownvoteFromCommentAsync(commentId, user.Id);

        if (!removedDownvote)
        {
            return Result<DownvoteCommentDTO>.Failure("Comment not found").WithCode(404);
        }

        return Result<DownvoteCommentDTO>.Success(new DownvoteCommentDTO(commentId, false));
    }

    public Task<Result<DownvotePostDTO>> DownvotePostAsync(Guid postId, UserDTO userDTO)
    {
        throw new NotImplementedException();
    }

    public Task<Result<DownvotePostDTO>> RemoveDownvoteFromPostAsync(Guid postId, UserDTO userDTO)
    {
        throw new NotImplementedException();
    }
}