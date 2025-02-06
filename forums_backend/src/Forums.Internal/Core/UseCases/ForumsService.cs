using AutoMapper;
using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.Internal.API.DTOs.Forums;
using forums_backend.src.Forums.Internal.API.DTOs.Users;
using forums_backend.src.Forums.Internal.API.Public;
using forums_backend.src.Forums.Internal.Core.Domain;
using forums_backend.src.Forums.Internal.Core.Domain.RepositoryInterfaces;

namespace forums_backend.src.Forums.Internal.Core.UseCases;

public class ForumsService : IForumsService
{
    private readonly IForumsRepository _forumsRepository;
    private readonly IMapper _mapper;

    public ForumsService(IForumsRepository forumsRepository, IMapper mapper)
    {
        _forumsRepository = forumsRepository;
        _mapper = mapper;
    }

    public async Task<Result<ForumDTO>> CreateForumAsync(CreateForumDTO createForumDTO, UserDTO userDTO)
    {
        Forum forum = new(createForumDTO.Name, createForumDTO.Description);

        User user = new(userDTO.Id, userDTO.Email, userDTO.Role, userDTO.Username);

        await _forumsRepository.AddAsync(forum, user);

        ForumDTO forumDTO = _mapper.Map<ForumDTO>(forum);

        return Result<ForumDTO>.Success(forumDTO);
    }

    public async Task<Result<IEnumerable<ForumDTO>>> GetForumsAsync()
    {
        var forums = await _forumsRepository.GetAllAsync();

        var forumDTOs = _mapper.Map<IEnumerable<ForumDTO>>(forums);

        return Result<IEnumerable<ForumDTO>>.Success(forumDTOs);
    }
}