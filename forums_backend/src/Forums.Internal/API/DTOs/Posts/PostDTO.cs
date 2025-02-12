using forums_backend.src.Forums.Internal.API.DTOs.Comments;
using forums_backend.src.Forums.Internal.API.DTOs.Forums;
using forums_backend.src.Forums.Internal.API.DTOs.Users;

namespace forums_backend.src.Forums.Internal.API.DTOs.Posts;

public class PostDTO
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsLocked { get; set; }
    public int NumberOfUpvotes { get; set; }
    public int NumberOfDownvotes { get; set; }
    public int NumberOfComments { get; set; }
    public bool IsUpvoted { get; set; }
    public bool IsDownvoted { get; set; }
    public UserDTO Author { get; set; } = null!;
    public ForumDTO Forum { get; set; } = null!;
    public PostDTO() { }
}