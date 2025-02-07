namespace forums_backend.src.Forums.Internal.Core.Domain;

public class Comment
{
    public Guid Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
    public int NumberOfUpvotes { get; set; } = 0;
    public int NumberOfDownvotes { get; set; } = 0;

    public Comment() { }
}