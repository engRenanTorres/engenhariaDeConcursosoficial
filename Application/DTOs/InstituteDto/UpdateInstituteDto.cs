using System.ComponentModel.DataAnnotations;
using Domain.Entities;

namespace Application.DTOs.Institute;

public class UpdateInstituteDto
{
  [Required]
  public Guid Id { get; set; }
  public string? Name { get; set; }
  public string? About { get; set; }
  public string? Contact { get; set; }
}
