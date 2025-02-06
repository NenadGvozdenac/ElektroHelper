using AutoMapper;
using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.Internal.API.DTOs.Users;
using forums_backend.src.Forums.Internal.API.Public;
using forums_backend.src.Forums.Internal.Core.Domain;
using forums_backend.src.Forums.Internal.Core.Domain.RepositoryInterfaces;

namespace forums_backend.src.Forums.Internal.Core.UseCases;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<Result<UserResponseDTO>> CreateUserAsync(UserDTO userDTO)
    {
        User user = new(userDTO.Id, userDTO.Email, userDTO.Role, userDTO.Username);

        User? createdUser = await _userRepository.AddAsync(user);

        if (createdUser == null)
        {
            return Result<UserResponseDTO>.Failure("Couldn't create user");
        }

        var createrUserDTO = _mapper.Map<UserResponseDTO>(createdUser);

        return Result<UserResponseDTO>.Success(createrUserDTO);
    }
}