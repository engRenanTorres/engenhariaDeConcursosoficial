using Apllication.DTOs;
using Apllication.DTOs.Users;
using Domain.Entities;

namespace Apllication.Services;

public interface IAuthService
{
  public Task<LogedUserInfoDto?> Login(LoginDTO loginDTO);
  public string CreateToken(AppUser user, IList<string> role);
  public Task<LogedUserInfoDto> Register(CreateUserDto createUserDTO);
  public Task<LogedUserInfoDto> GetCurrentUser(string email);
  public Task AddRole(RoleDto roleDto);
  public Task RemoveRole(RoleDto roleDto);
  Task<LogedUserInfoDto> Update(UpdateUserDto userDto);
  Task<LogedUserInfoDto?> RefreshToken(string? authEmail);
}
