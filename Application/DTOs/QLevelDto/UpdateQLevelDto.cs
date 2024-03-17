using System.ComponentModel.DataAnnotations;
using Domain.Entities;

namespace Application.DTOs.QLevel;

public class UpdateQLevelDto
{
  [Required]
  public Guid Id { get; set; }
  public string? Name { get; set; }
  public string? About { get; set; }
}
