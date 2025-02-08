namespace forums_backend.src.Forums.Internal.API.DTOs.Forums;

public class ForumDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsQuarantied { get; set; }
    public int NumberOfPosts { get; set; }

    public ForumDTO() { }
}