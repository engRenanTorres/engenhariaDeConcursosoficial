using System.ComponentModel.DataAnnotations;
using Domain.Entities;

namespace Application.DTOs.StudyArea;

public class UpdateAreaDto
{
  [Required]
  public Guid Id { get; set; }
  public string? Name { get; set; }
  public string? About { get; set; }
}
