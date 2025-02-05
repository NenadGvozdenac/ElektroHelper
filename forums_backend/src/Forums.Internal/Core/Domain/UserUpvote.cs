namespace forums_backend.src.Forums.Internal.Core.Domain;

public class UserUpvote {
    public User User { get; set; }
    public DateTime UpvotedAt { get; set; }

    public UserUpvote(User user, DateTime upvotedAt)
    {
        User = user;
        UpvotedAt = upvotedAt;
    }
}