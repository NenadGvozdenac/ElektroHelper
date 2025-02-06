namespace forums_backend.src.Forums.Internal.API.DTOs.Forums;

public record CreateForumDTO
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; init; } = string.Empty;

    public CreateForumDTO() { }

    public CreateForumDTO(string name, string description)
    {
        Name = name;
        Description = description;
    }
}
