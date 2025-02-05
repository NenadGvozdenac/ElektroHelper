namespace forums_backend.src.Forums.Internal.Core.Domain;

public class CommentWithUpvotes {
    public Comment Comment { get; set; }
    public IEnumerable<UserUpvote> Upvotes { get; set; }
    public int UpvotesCount { get; set; }

    public CommentWithUpvotes(Comment comment, IEnumerable<UserUpvote> upvotes)
    {
        Comment = comment;
        Upvotes = upvotes;
        UpvotesCount = upvotes.Count();
    }
}