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
    private readonly IDownvoteCommentRepository _downvoteCommentRepository;
    private readonly IDownvotePostRepository _downvotePostRepository;

    public UpvoteService(IUpvotePostRepository upvotePostRepository, IUpvoteCommentRepository upvoteCommentRepository, IDownvoteCommentRepository downvoteCommentRepository, IDownvotePostRepository downvotePostRepository)
    {
        _upvotePostRepository = upvotePostRepository;
        _upvoteCommentRepository = upvoteCommentRepository;
        _downvoteCommentRepository = downvoteCommentRepository;
        _downvotePostRepository = downvotePostRepository;
    }

    public async Task<Result<UpvoteCommentDTO>> UpvoteCommentAsync(Guid commentId, UserDTO userDTO)
    {
        User user = new(userDTO.Id, userDTO.Email, userDTO.Role, userDTO.Username);

        var userUpvotedComment = await _upvoteCommentRepository.UserUpvotedCommentAsync(commentId, user.Id);

        if(userUpvotedComment) {
            return Result<UpvoteCommentDTO>.Failure("User already upvoted this comment").WithCode(400);
        }

        await _downvoteCommentRepository.RemoveDownvoteFromCommentIfExistsAsync(commentId, user.Id);

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

    public async Task<Result<UpvotePostDTO>> UpvotePostAsync(Guid postId, UserDTO userDTO)
    {
        var user = new User(userDTO.Id, userDTO.Email, userDTO.Role, userDTO.Username);

        var userUpvotedPost = await _upvotePostRepository.UserUpvotedPostAsync(postId, user.Id);

        if(userUpvotedPost) {
            return Result<UpvotePostDTO>.Failure("User already upvoted this post").WithCode(400);
        }

        await _downvotePostRepository.RemoveDownvoteFromPostIfExistsAsync(postId, user.Id);

        var upvoteAdded = await _upvotePostRepository.AddUpvoteToPostAsync(postId, user);

        if(!upvoteAdded) {
            return Result<UpvotePostDTO>.Failure("Post not found").WithCode(404);
        }

        return Result<UpvotePostDTO>.Success(new UpvotePostDTO(postId, true));
    }

    public async Task<Result<UpvotePostDTO>> RemoveUpvoteFromPostAsync(Guid postId, UserDTO userDTO)
    {
        var user = new User(userDTO.Id, userDTO.Email, userDTO.Role, userDTO.Username);

        var userUpvotedPost = await _upvotePostRepository.UserUpvotedPostAsync(postId, user.Id);

        if(!userUpvotedPost) {
            return Result<UpvotePostDTO>.Failure("User did not upvote this post").WithCode(400);
        }

        var removedUpvote = await _upvotePostRepository.RemoveUpvoteFromPostAsync(postId, user);

        if(!removedUpvote) {
            return Result<UpvotePostDTO>.Failure("Post not found").WithCode(404);
        }

        return Result<UpvotePostDTO>.Success(new UpvotePostDTO(postId, false));
    }
}