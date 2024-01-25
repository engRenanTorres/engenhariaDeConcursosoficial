using Apllication.DTOs;
using Domain.Entities;
using Domain.Enums;

namespace Apllication.Services;

public interface IAuthService
{
  public Task<UserDto?> Login(LoginDTO loginDTO);
  public string CreateToken(AppUser user);
  public Task<UserDto> Register(CreateUserDto createUserDTO);
  public Task<UserDto> GetCurrentUser(string email);
  public Task AddRole(RoleDto roleDto);
  public Task RemoveRole(RoleDto roleDto);
  //string? RefreshToken(string? authUserId, Roles role);
}
