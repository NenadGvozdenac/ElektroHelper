namespace forums_backend.src.Forums.Internal.Core.Domain;

public class User {
    public string Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public bool IsBanned { get; set; } = false;
    public string? ReasonForBan { get; set; } = string.Empty;
    public bool IsDeleted { get; set; } = false;

    public User() {}
    public User(string id, string email, string role, string username) {
        Id = id;
        Email = email;
        Role = role;
        Username = username;
    }

    public User(string id, string email, string role, string username, bool isBanned, string reasonForBan, bool isDeleted) {
        Id = id;
        Email = email;
        Role = role;
        Username = username;
        IsBanned = isBanned;
        ReasonForBan = reasonForBan;
        IsDeleted = isDeleted;
    }
}