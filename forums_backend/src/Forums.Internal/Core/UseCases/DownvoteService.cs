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
    private readonly IUpvotePostRepository _upvotePostRepository;
    private readonly IDownvoteCommentRepository _downvoteCommentRepository;
    private readonly IUpvoteCommentRepository _upvoteCommentRepository;

    public DownvoteService(IDownvoteCommentRepository downvoteCommentRepository, IDownvotePostRepository downvotePostRepository, IUpvoteCommentRepository upvoteCommentRepository, IUpvotePostRepository upvotePostRepository)
    {
        _downvoteCommentRepository = downvoteCommentRepository;
        _downvotePostRepository = downvotePostRepository;
        _upvoteCommentRepository = upvoteCommentRepository;
        _upvotePostRepository = upvotePostRepository;
    }

    public async Task<Result<DownvoteCommentDTO>> DownvoteCommentAsync(Guid commentId, UserDTO userDTO)
    {
        User user = new(userDTO.Id, userDTO.Email, userDTO.Role, userDTO.Username);

        var userDownvotedComment = await _downvoteCommentRepository.UserDownvotedCommentAsync(commentId, user.Id);

        if (userDownvotedComment)
        {
            return Result<DownvoteCommentDTO>.Failure("User already downvoted this comment").WithCode(400);
        }

        await _upvoteCommentRepository.RemoveUpvoteFromCommentIfExistsAsync(commentId, user.Id);

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

    public async Task<Result<DownvotePostDTO>> DownvotePostAsync(Guid postId, UserDTO userDTO)
    {
        User user = new(userDTO.Id, userDTO.Email, userDTO.Role, userDTO.Username);

        var userDownvotedPost = await _downvotePostRepository.UserDownvotedPostAsync(postId, user.Id);

        if(userDownvotedPost)
        {
            return Result<DownvotePostDTO>.Failure("User already downvoted this post").WithCode(400);
        }

        await _upvotePostRepository.RemoveUpvoteFromPostIfExistsAsync(postId, user.Id);

        var downvoteAdded = await _downvotePostRepository.AddDownvoteToPostAsync(postId, user.Id);

        if (!downvoteAdded)
        {
            return Result<DownvotePostDTO>.Failure("Post not found").WithCode(404);
        }

        return Result<DownvotePostDTO>.Success(new DownvotePostDTO(postId, true));
    }

    public async Task<Result<DownvotePostDTO>> RemoveDownvoteFromPostAsync(Guid postId, UserDTO userDTO)
    {
        User user = new(userDTO.Id, userDTO.Email, userDTO.Role, userDTO.Username);

        var userDownvotedPost = await _downvotePostRepository.UserDownvotedPostAsync(postId, user.Id);

        if (!userDownvotedPost)
        {
            return Result<DownvotePostDTO>.Failure("User did not downvote this post").WithCode(400);
        }

        var removedDownvote = await _downvotePostRepository.RemoveDownvoteFromPostAsync(postId, user.Id);

        if (!removedDownvote)
        {
            return Result<DownvotePostDTO>.Failure("Post not found").WithCode(404);
        }

        return Result<DownvotePostDTO>.Success(new DownvotePostDTO(postId, false));
    }
}