namespace forums_backend.src.Forums.Internal.Core.Domain;

public class PostVoting {
    public Post Post { get; set; } = new();
    public bool IsUpvoted { get; set; }
    public bool IsDownvoted { get; set; }

    public PostVoting() {}
}