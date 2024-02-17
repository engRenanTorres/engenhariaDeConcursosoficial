using System.Security.Claims;
using Apllication.DTOs;
using Apllication.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
  private readonly IAuthService _authService;

  public AuthController(IAuthService authService)
  {
    _authService = authService;
  }

  [AllowAnonymous]
  [HttpPost("login")]
  public async Task<ActionResult<UserDto>> Login([FromBody] LoginDTO loginDTO)
  {
    var loginResult = await _authService.Login(loginDTO);
    return loginResult == null ? Unauthorized() : Ok(loginResult);
  }

  [AllowAnonymous]
  [HttpPost("Register")]
  public async Task<ActionResult<UserDto>> Register([FromBody] CreateUserDto userDto)
  {
    return await _authService.Register(userDto);
  }

  [Authorize(Roles = "Admin")]
  [HttpPost("AddRole")]
  public async Task<ActionResult> AddRole([FromBody] RoleDto roleDto)
  {
    await _authService.AddRole(roleDto);
    return NoContent();
  }

  [Authorize(Policy = "IsActivityHost")]
  [HttpPost("Update")]
  public async Task<ActionResult> Update([FromBody] RoleDto roleDto)
  {
    throw new NotImplementedException();
  }

  [Authorize(Roles = "Admin")]
  [HttpPost("RemoveRole")]
  public async Task<ActionResult<UserDto>> RemoveRole([FromBody] RoleDto roleDto)
  {
    await _authService.RemoveRole(roleDto);
    return NoContent();
  }

  [HttpGet]
  public async Task<ActionResult<UserDto>> GetCurrentUser()
  {
    var email = User.FindFirstValue(ClaimTypes.Email);
    return await _authService.GetCurrentUser(email);
  }
}
