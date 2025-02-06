namespace forums_backend.src.Forums.Internal.API.DTOs.Rating.Posts;

public class UpvotePostDTO
{
    public Guid PostId { get; set; }
    public int Upvotes { get; set; }
    public bool IsUpvoted { get; set; }

    public UpvotePostDTO() { }

    public UpvotePostDTO(Guid postId, int upvotes, bool isUpvoted)
    {
        PostId = postId;
        Upvotes = upvotes;
        IsUpvoted = isUpvoted;
    }
}