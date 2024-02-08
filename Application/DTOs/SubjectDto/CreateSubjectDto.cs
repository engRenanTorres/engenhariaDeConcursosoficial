using System.ComponentModel.DataAnnotations;
using Application.DTOs;

namespace Apllication.DTOs;

public class CreateSubjectDto
{
  [Required]
  public required string Name { get; set; } = "";
  public string About { get; set; } = "";

  [Required]
  public required Guid AreaId { get; set; }
}
