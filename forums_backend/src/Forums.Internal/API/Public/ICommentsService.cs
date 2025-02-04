using forums_backend.src.Forums.Internal.API.DTOs.Comments;
using forums_backend.src.Forums.Internal.API.DTOs.Users;

namespace forums_backend.src.Forums.Internal.API.Public;

public interface ICommentsService {
    public Task<CommentDTO> CreateCommentAsync(CreateCommentDTO createCommentDTO, UserDTO user);
    public Task<PostAndCommentsDTO> GetPostAndItsCommentsAsync(Guid postId);
    public Task<IEnumerable<CommentDTO>> GetMyCommentsAsync(UserDTO user);   
}