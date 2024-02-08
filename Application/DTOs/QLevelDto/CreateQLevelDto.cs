using System.ComponentModel.DataAnnotations;
using Application.DTOs;

namespace Apllication.DTOs.QLevel;

public class CreateQLevelDto
{
  [Required]
  public required string Name { get; set; } = "";
  public string About { get; set; } = "";
}
