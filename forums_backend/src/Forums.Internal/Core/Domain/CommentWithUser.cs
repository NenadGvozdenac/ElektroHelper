namespace forums_backend.src.Forums.Internal.Core.Domain;

public class CommentWithUser {
    public Comment Comment { get; }
    public User User { get; }

    public CommentWithUser(Comment comment, User user)
    {
        Comment = comment;
        User = user;
    }
}