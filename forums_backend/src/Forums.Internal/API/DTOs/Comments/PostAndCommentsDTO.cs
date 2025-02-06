using forums_backend.src.Forums.Internal.API.DTOs.Posts;
using forums_backend.src.Forums.Internal.API.DTOs.Users;

namespace forums_backend.src.Forums.Internal.API.DTOs.Comments;

public class PostAndCommentsDTO
{
    public PostDTO Post { get; set; } = null!;
    public IEnumerable<CommentWithUserAndUpvotesDTO> Comments { get; set; }
    public List<UserDTO> Upvotes { get; set; } = new();
    public List<UserDTO> Downvotes { get; set; } = new();
    public UserDTO OriginalPoster { get; set; } = new();
    public int UpvotesCount => Upvotes.Count;
    public int DownvotesCount => Downvotes.Count;

    public PostAndCommentsDTO()
    {
        Comments = new List<CommentWithUserAndUpvotesDTO>();
    }

    public PostAndCommentsDTO(PostDTO post, IEnumerable<CommentWithUserAndUpvotesDTO> comments, List<UserDTO> upvoters, List<UserDTO> downvoters, UserDTO originalPoster)
    {
        Post = post;
        Comments = comments;
        Upvotes = upvoters;
        Downvotes = downvoters;
        OriginalPoster = originalPoster;
    }
}