namespace forums_backend.src.Forums.BuildingBlocks.Infrastructure;

public class UserDTO
{
    public string Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;

    public UserDTO() { }

    public UserDTO(string id, string email, string role, string username)
    {
        Id = id;
        Email = email;
        Role = role;
        Username = username;
    }
}

public class UserResponseDTO
{
    public string Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public bool IsBanned { get; set; } = false;
    public string? ReasonForBan { get; set; } = null;
    public bool IsDeleted { get; set; } = false;

    public UserResponseDTO() { }

    public UserResponseDTO(string id, string email, string role, string username, bool isBanned, string? reasonForBan, bool isDeleted)
    {
        Id = id;
        Email = email;
        Role = role;
        Username = username;
        IsBanned = isBanned;
        ReasonForBan = reasonForBan;
        IsDeleted = isDeleted;
    }
}