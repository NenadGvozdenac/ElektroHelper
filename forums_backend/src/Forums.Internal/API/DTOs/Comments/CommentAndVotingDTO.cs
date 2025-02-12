using forums_backend.src.Forums.Internal.API.DTOs.Comments;
using forums_backend.src.Forums.Internal.API.DTOs.Users;

public class CommentAndVotingDTO {
    public CommentDTO Comment { get; set; } = null!;
    public UserDTO Author { get; set; } = null!;
    public bool IsUpvoted { get; set; }
    public bool IsDownvoted { get; set; }

    public CommentAndVotingDTO() { }
}