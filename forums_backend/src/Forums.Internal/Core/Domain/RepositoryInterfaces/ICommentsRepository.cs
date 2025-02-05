namespace forums_backend.src.Forums.Internal.Core.Domain.RepositoryInterfaces;

public interface ICommentsRepository {
    public Task<Comment?> AddAsync(Comment comment, Guid postId, User user);
    public Task<PostAndComments?> GetPostAndItsCommentsAsync(Guid postId);
    public Task<IEnumerable<Comment>> GetAllAsync();
    public Task<Comment?> GetByIdAsync(Guid commentId);
    public Task<IEnumerable<Comment>> GetMyCommentsAsync(User user);
}