namespace forums_backend.src.Forums.Internal.API.DTOs.Posts;

public class CreatePostDTO
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public Guid ForumId { get; set; }

    public CreatePostDTO() { }

    public CreatePostDTO(string title, string content, Guid forumId)
    {
        Title = title;
        Content = content;
        ForumId = forumId;
    }
}