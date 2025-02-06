namespace forums_backend.src.Forums.Internal.API.DTOs.Posts;

public class PostDTO
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsLocked { get; set; }

    public PostDTO() { }

    public PostDTO(Guid id, string title, string content, DateTime createdAt, bool isDeleted, bool isLocked)
    {
        Id = id;
        Title = title;
        Content = content;
        CreatedAt = createdAt;
        IsDeleted = isDeleted;
        IsLocked = isLocked;
    }
}