namespace forums_backend.src.Forums.Internal.Core.Domain;

public class PostAndCommentsWithUpvotesAndDownvotes {
    public Post Post { get; set; }
    public User Creator { get; set; }
    public IEnumerable<CommentWithUpvotesAndDownvotes> Comments { get; set; }

    public PostAndCommentsWithUpvotesAndDownvotes(Post post, User creator, IEnumerable<CommentWithUpvotesAndDownvotes> comments) {
        Post = post;
        Comments = comments;
        Creator = creator;
    }
}