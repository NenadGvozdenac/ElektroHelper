using forums_backend.src.Forums.Internal.API.DTOs.Forums;
using forums_backend.src.Forums.Internal.API.DTOs.Users;

namespace forums_backend.src.Forums.Internal.API.Public;

public interface IForumsService {
    public Task<ForumDTO> CreateForumAsync(CreateForumDTO createCategoryDTO, UserDTO user);
    public Task<IEnumerable<ForumDTO>> GetForumsAsync();
}