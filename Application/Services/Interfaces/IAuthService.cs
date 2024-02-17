using Apllication.DTOs;
using Apllication.DTOs.Users;
using Domain.Entities;

namespace Apllication.Services;

public interface IAuthService
{
  public Task<UserDto?> Login(LoginDTO loginDTO);
  public string CreateToken(AppUser user);
  public Task<UserDto> Register(CreateUserDto createUserDTO);
  public Task<UserDto> GetCurrentUser(string email);
  public Task AddRole(RoleDto roleDto);
  public Task RemoveRole(RoleDto roleDto);
  Task<UserDto> Update(UpdateUserDto userDto);
  //string? RefreshToken(string? authUserId, Roles role);
}
