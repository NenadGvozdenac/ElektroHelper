using forums_backend.src.Forums.Internal.API.DTOs.Posts;

namespace forums_backend.src.Forums.Internal.API.DTOs.Comments;

public class PostAndCommentsDTO
{
    public PostDTO Post { get; set; } = null!;
    public IEnumerable<CommentWithUserAndUpvotesDTO> Comments { get; set; }

    public PostAndCommentsDTO()
    {
        Comments = new List<CommentWithUserAndUpvotesDTO>();
    }

    public PostAndCommentsDTO(PostDTO post, IEnumerable<CommentWithUserAndUpvotesDTO> comments)
    {
        Post = post;
        Comments = comments;
    }
}