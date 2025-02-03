namespace forums_backend.src.Forums.Internal.Core.Domain;

public class Post {
    public Guid Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Content { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }

    public Post(Guid id, string title, string content, DateTime createdAt) {
        Id = id;
        Title = title;
        Content = content;
        CreatedAt = createdAt;
    }

    public Post(Guid id, string title, string content) {
        Id = id;
        Title = title;
        Content = content;
        CreatedAt = DateTime.UtcNow;
    }

    public Post(string title, string content) {
        Id = Guid.NewGuid();
        Title = title;
        Content = content;
        CreatedAt = DateTime.UtcNow;
    }
}