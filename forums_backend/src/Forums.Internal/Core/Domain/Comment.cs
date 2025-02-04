namespace forums_backend.src.Forums.Internal.Core.Domain;

public class Comment {
    public Guid Id { get; private set; }
    public string Content { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }

    public Comment(string content) {
        Id = Guid.NewGuid();
        Content = content;
        CreatedAt = DateTime.UtcNow;
    }

    public Comment(Guid id, string content, DateTime createdAt) {
        Id = id;
        Content = content;
        CreatedAt = createdAt;
    }
}