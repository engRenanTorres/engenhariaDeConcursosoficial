using System.ComponentModel.DataAnnotations;

namespace Apllication.DTOs;

public class RoleDto
{
  [Required]
  public string UserEmail { get; set; } = "";

  [Required]
  public string UserRole { get; set; } = "";
}
