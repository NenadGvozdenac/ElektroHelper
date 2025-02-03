namespace forums_backend.src.Forums.Internal.Core.Domain;

public class ForumAndPosts {
    public Forum Forum { get; private set; }
    public IEnumerable<Post> Posts { get; private set; }

    public ForumAndPosts(Forum forum, IEnumerable<Post> posts) {
        Forum = forum;
        Posts = posts;
    }
}