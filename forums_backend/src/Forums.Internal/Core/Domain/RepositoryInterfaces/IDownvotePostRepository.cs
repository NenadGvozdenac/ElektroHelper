
namespace forums_backend.src.Forums.Internal.Core.Domain.RepositoryInterfaces;

public interface IDownvotePostRepository
{
    public Task<bool> AddDownvoteToPostAsync(Guid postId, string userId);
    public Task<bool> RemoveDownvoteFromPostAsync(Guid postId, string userId);
    public Task<bool> RemoveDownvoteFromPostIfExistsAsync(Guid postId, string id);
    public Task<bool> UserDownvotedPostAsync(Guid postId, string userId);
}