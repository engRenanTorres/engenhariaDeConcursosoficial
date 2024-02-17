using System.ComponentModel.DataAnnotations;

namespace Apllication.DTOs.Users;

public class UpdateUserDto
{
  public string? DisplayName { get; set; }

  [EmailAddress]
  public string? Email { get; set; }

  /*[RegularExpression(
    "(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{4,8}$",
    ErrorMessage = "Password must have at least one digit, one lower case and uper case letter. 4 > Lenght > 8 "
  )]*/
  public string? Password { get; set; }

  public string? Username { get; set; }
  public string? Bio { get; set; } = "";
  public string? Website { get; set; }
}
