using forums_backend.src.Forums.Internal.API.DTOs.Users;

namespace forums_backend.src.Forums.Internal.Core.Domain.RepositoryInterfaces;

public interface ICommentsRepository {
    public Task<Comment?> AddAsync(Comment comment, Guid postId, User user);
    public Task<IEnumerable<Comment>> GetAllAsync();
    public Task<Comment?> GetByIdAsync(Guid commentId);
    public Task<IEnumerable<Comment>> GetMyCommentsAsync(User user);
    public Task<IEnumerable<CommentAndVoting>> GetCommentsForPost(Guid postId, UserDTO userDTO);
}