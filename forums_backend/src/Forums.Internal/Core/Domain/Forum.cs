namespace forums_backend.src.Forums.Internal.Core.Domain;

public class Forum {
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;

    public Forum(Guid id, string name, string description) {
        Id = id;
        Name = name;
        Description = description;
    }

    public Forum(string name, string description) {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
    }
}