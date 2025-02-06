namespace forums_backend.src.Forums.Internal.API.DTOs.Forums;

public class ForumDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsQuarantied { get; set; }

    public ForumDTO() { }

    public ForumDTO(Guid id, string name, string description, DateTime createdAt, bool isDeleted, bool isQuarantied)
    {
        Id = id;
        Name = name;
        Description = description;
        CreatedAt = createdAt;
        IsDeleted = isDeleted;
        IsQuarantied = isQuarantied;
    }
}