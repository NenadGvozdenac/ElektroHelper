using forums_backend.src.Forums.Internal.API.DTOs.Users;

namespace forums_backend.src.Forums.Internal.API.DTOs.Comments;
public class CommentDTO
{
    public Guid Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public int NumberOfUpvotes { get; set; }
    public int NumberOfDownvotes { get; set; }

    public CommentDTO() { }

    public CommentDTO(Guid id, string content, DateTime createdAt, bool isDeleted)
    {
        Id = id;
        Content = content;
        CreatedAt = createdAt;
        IsDeleted = isDeleted;
    }
}

public class CommentWithUserDTO
{
    public Guid Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public UserDTO User { get; set; } = null!;

    public CommentWithUserDTO() { }

    public CommentWithUserDTO(Guid id, string content, DateTime createdAt, UserDTO user)
    {
        Id = id;
        Content = content;
        CreatedAt = createdAt;
        User = user;
    }
}

public class CommentWithUserAndUpvotesDTO
{
    public Guid Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public UserDTO Creator { get; set; } = null!;
    public List<UserDTO> Upvotes { get; set; } = new();
    public List<UserDTO> Downvotes { get; set; } = new();

    public int UpvotesCount => Upvotes.Count;
    public int DownvotesCount => Downvotes.Count;

    public CommentWithUserAndUpvotesDTO()
    {
        Upvotes = new List<UserDTO>();
        Downvotes = new List<UserDTO>();
    }

    public CommentWithUserAndUpvotesDTO(Guid id, string content, DateTime createdAt, UserDTO creator, List<UserDTO> upvotes, List<UserDTO> downvotes)
    {
        Id = id;
        Content = content;
        CreatedAt = createdAt;
        Creator = creator;
        Upvotes = upvotes;
        Downvotes = downvotes;
    }
}