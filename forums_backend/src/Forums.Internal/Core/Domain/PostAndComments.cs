namespace forums_backend.src.Forums.Internal.Core.Domain;

public class PostAndComments {
    public Post Post { get; private set; }
    public IEnumerable<Comment> Comments { get; private set; }

    public PostAndComments(Post post, IEnumerable<Comment> comments) {
        Post = post;
        Comments = comments;
    }
}