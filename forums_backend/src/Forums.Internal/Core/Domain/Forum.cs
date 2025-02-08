namespace forums_backend.src.Forums.Internal.Core.Domain;

public class Forum {
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
    public bool IsQuarantined { get; set; } = false;
    public int NumberOfPosts { get; set; } = 0;

    public Forum() {}

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