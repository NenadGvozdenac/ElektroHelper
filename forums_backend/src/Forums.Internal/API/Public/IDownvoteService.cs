using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.Internal.API.DTOs.Rating.Comments;
using forums_backend.src.Forums.Internal.API.DTOs.Rating.Posts;
using forums_backend.src.Forums.Internal.API.DTOs.Users;

namespace forums_backend.src.Forums.Internal.API.Public;

public interface IDownvoteService {
    public Task<Result<DownvoteCommentDTO>> DownvoteCommentAsync(Guid commentId, UserDTO user);
    public Task<Result<DownvotePostDTO>> DownvotePostAsync(Guid postId, UserDTO user);
    public Task<Result<DownvoteCommentDTO>> RemoveDownvoteFromCommentAsync(Guid commentId, UserDTO user);
    public Task<Result<DownvotePostDTO>> RemoveDownvoteFromPostAsync(Guid postId, UserDTO user);
}