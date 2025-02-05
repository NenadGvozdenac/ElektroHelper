namespace forums_backend.src.Forums.Internal.Core.Domain.RepositoryInterfaces;

public interface IUpvoteCommentRepository {
    public Task<bool> AddUpvoteToCommentAsync(Guid commentId, string userId);
    public Task<bool> RemoveUpvoteFromCommentAsync(Guid commentId, string userId);
    public Task<CommentWithUpvotes?> GetCommentWithUpvotesAsync(Guid commentId);
    public Task<bool> UserUpvotedCommentAsync(Guid commentId, string userId);
    public Task<bool> RemoveUpvoteFromCommentIfExistsAsync(Guid commentId, string id);
}