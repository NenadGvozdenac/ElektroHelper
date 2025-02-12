using forums_backend.src.Forums.Internal.Core.Domain;

public class CommentAndVoting {
    public Comment Comment { get; set; }
    public User Author { get; set; }
    public bool IsUpvoted { get; set; }
    public bool IsDownvoted { get; set; }

    public CommentAndVoting() {}
}