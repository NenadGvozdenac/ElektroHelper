using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.Internal.API.DTOs.Comments;
using forums_backend.src.Forums.Internal.API.DTOs.Users;
using Microsoft.AspNetCore.Mvc;

namespace forums_backend.src.Forums.Internal.API.Public;

public interface ICommentsService {
    public Task<Result<CommentDTO>> CreateCommentAsync(CreateCommentDTO createCommentDTO, UserDTO user);
    public Task<Result<IEnumerable<CommentDTO>>> GetMyCommentsAsync(UserDTO user);
    public Task<Result<IEnumerable<CommentAndVotingDTO>>> GetCommentsForPost(Guid postId, UserDTO user);
}