namespace forums_backend.src.Forums.Internal.API.DTOs.Rating.Posts;

public class DownvotePostDTO
{
    public Guid PostId { get; set; }
    public bool IsUpvoted { get; set; }

    public DownvotePostDTO() { }

    public DownvotePostDTO(Guid postId, bool isUpvoted)
    {
        PostId = postId;
        IsUpvoted = isUpvoted;
    }
}