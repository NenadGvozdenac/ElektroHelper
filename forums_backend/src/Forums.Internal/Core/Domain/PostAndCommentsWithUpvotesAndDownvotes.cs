namespace forums_backend.src.Forums.Internal.Core.Domain;

public class PostAndCommentsWithUpvotesAndDownvotes {
    public Post Post { get; set; }
    public User Creator { get; set; }
    public IEnumerable<CommentWithUpvotesAndDownvotes> Comments { get; set; }
    public IEnumerable<UserUpvote> Upvoters { get; set; }
    public IEnumerable<UserDownvote> Downvoters { get; set; }
    public PostAndCommentsWithUpvotesAndDownvotes(Post post, User creator, IEnumerable<CommentWithUpvotesAndDownvotes> comments, IEnumerable<UserUpvote> upvoters, IEnumerable<UserDownvote> downvoters) {
        Post = post;
        Comments = comments;
        Creator = creator;
        Upvoters = upvoters;
        Downvoters = downvoters;
    }
}