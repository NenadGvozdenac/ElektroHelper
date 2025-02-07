namespace forums_backend.src.Forums.Internal.Core.Domain;

public class Post
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
    public bool IsLocked { get; set; } = false;
    public int NumberOfUpvotes { get; set; } = 0;
    public int NumberOfDownvotes { get; set; } = 0;

    public Post() { }

    public void SetNumberOfUpvotes(int numberOfUpvotes)
    {
        NumberOfUpvotes = numberOfUpvotes;
    }

    public void SetNumberOfDownvotes(int numberOfDownvotes)
    {
        NumberOfDownvotes = numberOfDownvotes;
    }
}