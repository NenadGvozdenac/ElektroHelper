namespace forums_backend.src.Forums.Internal.Core.Domain;

public class Forum {
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }

    public Forum(Guid id, string name, string description, DateTime createdAt) {
        Id = id;
        Name = name;
        Description = description;
        CreatedAt = createdAt;
    }

    public Forum(string name, string description) {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        
        Name = name;
        Description = description;
    }
}