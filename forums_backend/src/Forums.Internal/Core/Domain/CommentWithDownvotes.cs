namespace forums_backend.src.Forums.Internal.Core.Domain;

public class CommentWithDownvotes {
    public Comment Comment { get; set; }
    public IEnumerable<UserDownvote> Downvotes { get; set; }
    public int DownvotesCount { get; set; }

    public CommentWithDownvotes(Comment comment, IEnumerable<UserDownvote> downvotes)
    {
        Comment = comment;
        Downvotes = downvotes;
        DownvotesCount = downvotes.Count();
    }
}