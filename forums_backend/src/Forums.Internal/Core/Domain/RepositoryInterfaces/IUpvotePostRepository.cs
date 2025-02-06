
namespace forums_backend.src.Forums.Internal.Core.Domain.RepositoryInterfaces;

public interface IUpvotePostRepository
{
    public Task<bool> AddUpvoteToPostAsync(Guid postId, User user);
    public Task<bool> RemoveUpvoteFromPostAsync(Guid postId, User user);
    public Task<bool> UserUpvotedPostAsync(Guid postId, string id);
}