namespace forums_backend.src.Forums.Internal.Core.Domain;

public class Forum {
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }
    public bool IsDeleted { get; private set; } = false;
    public bool IsQuarantined { get; private set; } = false;

    public Forum() {}

    public Forum(Guid id, string name, string description, DateTime createdAt) {
        Id = id;
        Name = name;
        Description = description;
        CreatedAt = createdAt;
    }

    public Forum(Guid id, string name, string description, DateTime createdAt, bool isDeleted, bool isQuarantined) {
        Id = id;
        Name = name;
        Description = description;
        CreatedAt = createdAt;
        IsDeleted = isDeleted;
        IsQuarantined = isQuarantined;
    }

    public Forum(string name, string description) {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        
        Name = name;
        Description = description;
    }
}