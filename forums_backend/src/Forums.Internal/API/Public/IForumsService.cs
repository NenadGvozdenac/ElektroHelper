using forums_backend.src.Forums.Internal.API.DTOs.Forums;

namespace forums_backend.src.Forums.Internal.API.Public;

public interface IForumsService {
    public Task CreateForumAsync(CreateForumDTO createCategoryDTO);
    public Task<IEnumerable<ForumDTO>> GetForumsAsync();
}