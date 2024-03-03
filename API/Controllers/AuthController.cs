using System.Security.Claims;
using Apllication.DTOs;
using Apllication.Exceptions;
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
  public async Task<ActionResult<LogedUserInfoDto>> Login([FromBody] LoginDTO loginDTO)
  {
    var loginResult = await _authService.Login(loginDTO);
    return loginResult == null ? Unauthorized() : Ok(loginResult);
  }

  [AllowAnonymous]
  [HttpPost("Register")]
  public async Task<ActionResult<LogedUserInfoDto>> Register([FromBody] CreateUserDto userDto)
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
  public async Task<ActionResult<LogedUserInfoDto>> RemoveRole([FromBody] RoleDto roleDto)
  {
    await _authService.RemoveRole(roleDto);
    return NoContent();
  }

  [HttpGet]
  public async Task<ActionResult<LogedUserInfoDto>> GetCurrentUser()
  {
    var email =
      User.FindFirstValue(ClaimTypes.Email) ?? throw new NotFoundException("User not found.");

    return await _authService.GetCurrentUser(email);
  }

  [HttpGet("RefreshToken")]
  public async Task<IActionResult> RefreshToken()
  {
    string? userLogedId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    var user = User;

    if (userLogedId == null)
      return BadRequest("User is not Logged or dont has e-mail");

    var token = await _authService.RefreshToken(userLogedId);
    if (token == null)
      return NotFound();
    return Ok(token);
  }
}
