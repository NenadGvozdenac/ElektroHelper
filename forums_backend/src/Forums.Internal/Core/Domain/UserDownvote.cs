namespace forums_backend.src.Forums.Internal.Core.Domain;

public class UserDownvote {
    public User User { get; set; }
    public DateTime DownvotedAt { get; set; }

    public UserDownvote(User user, DateTime downvotedAt)
    {
        User = user;
        DownvotedAt = downvotedAt;
    }
}