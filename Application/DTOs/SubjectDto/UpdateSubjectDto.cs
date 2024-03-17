using System.ComponentModel.DataAnnotations;
using Domain.Entities;

namespace Application.DTOs.SubjectDto;

public class UpdateSubjectDto
{
  [Required]
  public Guid Id { get; set; }
  public string? Name { get; set; }

  [Required]
  public Guid? AreaId { get; set; }
  public string? About { get; set; }
}
