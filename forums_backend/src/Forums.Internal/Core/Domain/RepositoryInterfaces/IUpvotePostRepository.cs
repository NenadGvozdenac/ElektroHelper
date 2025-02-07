
namespace forums_backend.src.Forums.Internal.Core.Domain.RepositoryInterfaces;

public interface IUpvotePostRepository
{
    public Task<bool> AddUpvoteToPostAsync(Guid postId, User user);
    public Task<bool> RemoveUpvoteFromPostAsync(Guid postId, User user);
    public Task<bool> RemoveUpvoteFromPostIfExistsAsync(Guid postId, string id);
    public Task<bool> UserUpvotedPostAsync(Guid postId, string id);
}