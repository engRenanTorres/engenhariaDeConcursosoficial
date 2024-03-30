using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Apllication.DTOs;
using Apllication.DTOs.Users;
using Apllication.Exceptions;
using Apllication.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Apllication.Services;

public class AuthService : IAuthService
{
  private readonly UserManager<AppUser> _userManager;
  private readonly RoleManager<IdentityRole> _roleManager;
  private readonly IConfiguration _configuration;
  private readonly IUserAccessor _userAccessor;

  public AuthService(
    UserManager<AppUser> userManager,
    RoleManager<IdentityRole> roleManager,
    IConfiguration configuration,
    IUserAccessor userAccessor
  )
  {
    _userManager = userManager;
    _roleManager = roleManager;
    _configuration = configuration;
    _userAccessor = userAccessor;
  }

  public async Task<LogedUserInfoDto?> Login(LoginDTO loginDTO)
  {
    var user = await _userManager.FindByEmailAsync(loginDTO.Email);
    if (user == null)
      return null; //Unauthorized();
    var role = await _userManager.GetRolesAsync(user);

    var autorizeResult = await _userManager.CheckPasswordAsync(user, loginDTO.Password);
    if (autorizeResult)
    {
      return ParseUserDto(user, role);
    }
    return null; // Unauthorized();
  }

  private LogedUserInfoDto ParseUserDto(AppUser user, IList<string> role)
  {
    return new LogedUserInfoDto
    {
      Valid = true,
      Credentials =
      {
        DisplayName = user?.DisplayName ?? "",
        Token = this.CreateToken(user, role),
        Id = user.Id,
        RoleName = role[0].ToString()
        //Image= null,
      }
    };
  }

  public string CreateToken(AppUser user, IList<string> role)
  {
    var claims = new List<Claim>
    {
      new Claim(ClaimTypes.Name, user.UserName ?? ""),
      new Claim(ClaimTypes.NameIdentifier, user.Id),
      new Claim(ClaimTypes.Role, role[0].ToString() ?? ""),
    };

    var key = new SymmetricSecurityKey(
      Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:PasswordKey").Value + "salt")
    );
    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
    var tokenDescriptor = new SecurityTokenDescriptor
    {
      Subject = new ClaimsIdentity(claims),
      Expires = DateTime.UtcNow.AddDays(7),
      SigningCredentials = credentials
    };

    var tokenHandler = new JwtSecurityTokenHandler();

    var token = tokenHandler.CreateToken(tokenDescriptor);

    return tokenHandler.WriteToken(token);
  }

  public async Task<LogedUserInfoDto> Register(CreateUserDto userDto)
  {
    var checkDuplicatedUsername = await _userManager.Users.AnyAsync(u =>
      u.UserName == userDto.Username
    );
    if (checkDuplicatedUsername)
      throw new BadRequestException("Username is already taken!");
    var checkDuplicatedEmail = await _userManager.Users.AnyAsync(u => u.Email == userDto.Email);
    if (checkDuplicatedUsername)
      throw new BadRequestException("Email is already taken!");

    var user = new AppUser
    {
      DisplayName = userDto.DisplayName,
      Email = userDto.Email,
      UserName = userDto.Username,
      Bio = userDto.Bio ?? "",
    };

    var result = await _userManager.CreateAsync(user, userDto.Password);
    if (result.Succeeded)
    {
      if (await _roleManager.FindByNameAsync("User") == null)
        throw new Exception("Default user are not set");
      await _userManager.AddToRoleAsync(user, "User");
      return this.ParseUserDto(user, new List<string>() { "User" });
    }
    throw new BadRequestException("Error saving user", new() { Errors = result.Errors.ToString() });
  }

  public async Task<LogedUserInfoDto> Update(UpdateUserDto userDto)
  {
    var userName =
      _userAccessor.GetUsername() ?? throw new BadRequestException("You must be loged to create.");
    var user =
      await _userManager.FindByNameAsync(userName)
      ?? throw new BadRequestException("You must be loged to create.");
    var role = await _userManager.GetRolesAsync(user);
    var userUpdated = new AppUser
    {
      DisplayName = userDto.DisplayName ?? user.DisplayName,
      Email = userDto.Email ?? user.Email,
      UserName = userDto.Username ?? userDto.Username,
      Bio = userDto.Bio ?? user.Bio,
    };

    var result = await _userManager.UpdateAsync(userUpdated);
    if (result.Succeeded)
    {
      return this.ParseUserDto(user, role);
    }
    throw new BadRequestException(
      "Error updating user",
      new() { Errors = result.Errors.ToString() }
    );
  }

  public async Task<LogedUserInfoDto> UpdatePassword(string currentPassword, string newPassword)
  {
    var userName =
      _userAccessor.GetUsername() ?? throw new BadRequestException("You must be loged to create.");
    var user =
      await _userManager.FindByNameAsync(userName)
      ?? throw new BadRequestException("You must be loged to create.");
    var role = await _userManager.GetRolesAsync(user);

    var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
    if (result.Succeeded)
    {
      return this.ParseUserDto(user, role);
    }
    throw new BadRequestException(
      "Error updating user",
      new() { Errors = result.Errors.ToString() }
    );
  }

  public async Task<LogedUserInfoDto> GetCurrentUser(string email)
  {
    var user =
      await _userManager.FindByEmailAsync(email ?? "")
      ?? throw new Exception("Auth User is not set!");
    var role = await _userManager.GetRolesAsync(user);
    return this.ParseUserDto(user, role);
  }

  public async Task AddRole(RoleDto roleDto)
  {
    var role =
      await _roleManager.FindByNameAsync(roleDto.UserRole)
      ?? throw new NotFoundException("This role does not exist!");
    var user =
      await _userManager.FindByEmailAsync(roleDto.UserEmail)
      ?? throw new NotFoundException("User does not exist for this e-mail!");

    await _userManager.AddToRoleAsync(user, roleDto.UserRole);
  }

  public async Task RemoveRole(RoleDto roleDto)
  {
    var role =
      await _roleManager.FindByNameAsync(roleDto.UserRole)
      ?? throw new NotFoundException("This role does not exist!");
    var user =
      await _userManager.FindByEmailAsync(roleDto.UserEmail)
      ?? throw new NotFoundException("User does not exist for this e-mail!");

    await _userManager.RemoveFromRoleAsync(user, roleDto.UserRole);
  }

  public async Task<LogedUserInfoDto?> RefreshToken(string? userId)
  {
    if (userId == null)
      return null;
    var user =
      await _userManager.FindByIdAsync(userId) ?? throw new NotFoundException("User is not logged");
    var role = await _userManager.GetRolesAsync(user);

    var token = this.ParseUserDto(user, role);
    return token;
  }
}
