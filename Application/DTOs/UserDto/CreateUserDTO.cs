using System.ComponentModel.DataAnnotations;

namespace Apllication.DTOs;

public class CreateUserDto
{
  [Required]
  public required string DisplayName { get; set; }

  [Required]
  [EmailAddress]
  public required string Email { get; set; }

  [Required]
  /*[RegularExpression(
    "(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{4,8}$",
    ErrorMessage = "Password must have at least one digit, one lower case and uper case letter. 4 > Lenght > 8 "
  )]*/
  public required string Password { get; set; }

  [Required]
  public required string Username { get; set; }
  public string? Bio { get; set; } = "";
  public string? Website { get; set; }
}
