namespace forums_backend.src.Forums.Internal.Core.Domain.RepositoryInterfaces;

public interface IDownvoteCommentRepository {
    public Task<bool> AddDownvoteToCommentAsync(Guid commentId, string userId);
    public Task<bool> RemoveDownvoteFromCommentAsync(Guid commentId, string userId);
    public Task<CommentWithDownvotes?> GetCommentWithDownvotesAsync(Guid commentId);
    public Task<bool> UserDownvotedCommentAsync(Guid commentId, string userId);
    public Task<bool> RemoveDownvoteFromCommentIfExistsAsync(Guid commentId, string id);
}