using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.Internal.API.DTOs.Rating.Comments;
using forums_backend.src.Forums.Internal.API.DTOs.Rating.Posts;
using forums_backend.src.Forums.Internal.API.DTOs.Users;

namespace forums_backend.src.Forums.Internal.API.Public;

public interface IUpvoteService {
    public Task<Result<UpvoteCommentDTO>> UpvoteCommentAsync(Guid commentId, UserDTO user);
    public Task<Result<UpvotePostDTO>> UpvotePostAsync(Guid postId, UserDTO user);
    public Task<Result<UpvoteCommentDTO>> RemoveUpvoteFromCommentAsync(Guid commentId, UserDTO user);
    public Task<Result<UpvotePostDTO>> RemoveUpvoteFromPostAsync(Guid postId, UserDTO user);
}