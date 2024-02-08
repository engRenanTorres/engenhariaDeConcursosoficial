using System.ComponentModel.DataAnnotations;
using Application.DTOs;

namespace Apllication.DTOs.Institute;

public class CreateInstituteDto
{
  [Required]
  public required string Name { get; set; } = "";
  public string About { get; set; } = "";

  public string Contact { get; set; } = "";
}
