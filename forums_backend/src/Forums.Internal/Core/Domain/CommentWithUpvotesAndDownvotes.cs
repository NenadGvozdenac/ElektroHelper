namespace forums_backend.src.Forums.Internal.Core.Domain;

public class CommentWithUpvotesAndDownvotes {
    public Comment Comment { get; set; }
    public User Creator { get; set; }
    public IEnumerable<UserUpvote> Upvotes { get; set; }
    public IEnumerable<UserDownvote> Downvotes { get; set; }
    public int UpvotesCount { get; set; }
    public int DownvotesCount { get; set; }

    public CommentWithUpvotesAndDownvotes(Comment comment, User creator, IEnumerable<UserUpvote> upvotes, IEnumerable<UserDownvote> downvotes)
    {
        Comment = comment;
        Upvotes = upvotes;
        Downvotes = downvotes;
        UpvotesCount = upvotes.Count();
        DownvotesCount = downvotes.Count();
        Creator = creator;
    }
}