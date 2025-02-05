using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.Internal.API.DTOs.Rating.Comments;
using forums_backend.src.Forums.Internal.API.DTOs.Rating.Posts;
using forums_backend.src.Forums.Internal.API.DTOs.Users;
using forums_backend.src.Forums.Internal.API.Public;
using forums_backend.src.Forums.Internal.Core.Domain;
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

    public async Task<Result<UpvoteCommentDTO>> UpvoteCommentAsync(Guid commentId, UserDTO userDTO)
    {
        User user = new(userDTO.Id, userDTO.Email, userDTO.Role, userDTO.Username);

        var userUpvotedComment = await _upvoteCommentRepository.UserUpvotedCommentAsync(commentId, user.Id);

        if(userUpvotedComment) {
            return Result<UpvoteCommentDTO>.Failure("User already upvoted this comment").WithCode(400);
        }

        // TODO:    Check if user downvoted the comment before upvoting it
        //          If user downvoted the comment, remove the downvote before adding the upvote

        var upvoteAdded = await _upvoteCommentRepository.AddUpvoteToCommentAsync(commentId, user.Id);

        if(!upvoteAdded) {
            return Result<UpvoteCommentDTO>.Failure("Comment not found").WithCode(404);
        }

        return Result<UpvoteCommentDTO>.Success(new UpvoteCommentDTO(commentId, true));
    }

    public async Task<Result<UpvoteCommentDTO>> RemoveUpvoteFromCommentAsync(Guid commentId, UserDTO userDTO)
    {
        User user = new(userDTO.Id, userDTO.Email, userDTO.Role, userDTO.Username);

        var userUpvotedComment = await _upvoteCommentRepository.UserUpvotedCommentAsync(commentId, user.Id);

        if(!userUpvotedComment) {
            return Result<UpvoteCommentDTO>.Failure("User did not upvote this comment").WithCode(400);
        }

        var removedUpvote = await _upvoteCommentRepository.RemoveUpvoteFromCommentAsync(commentId, user.Id);

        if(!removedUpvote) {
            return Result<UpvoteCommentDTO>.Failure("Comment not found").WithCode(404);
        }

        return Result<UpvoteCommentDTO>.Success(new UpvoteCommentDTO(commentId, false));
    }

    public Task<Result<UpvotePostDTO>> UpvotePostAsync(Guid postId, UserDTO userDTO)
    {
        throw new NotImplementedException();
    }

    public Task<Result<UpvotePostDTO>> RemoveUpvoteFromPostAsync(Guid postId, UserDTO userDTO)
    {
        throw new NotImplementedException();
    }
}