namespace forums_backend.src.Forums.Internal.API.DTOs.Posts;

public class ForumAndPostsDTO
{
    public Guid ForumId { get; set; }
    public string ForumTitle { get; set; } = string.Empty;
    public string ForumDescription { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public IEnumerable<PostDTO> Posts { get; set; }

    public ForumAndPostsDTO() { 
        Posts = new List<PostDTO>();
    }

    public ForumAndPostsDTO(Guid forumId, string forumTitle, string forumDescription, DateTime createdAt, IEnumerable<PostDTO> posts)
    {
        ForumId = forumId;
        ForumTitle = forumTitle;
        ForumDescription = forumDescription;
        CreatedAt = createdAt;
        Posts = posts;
    }
}