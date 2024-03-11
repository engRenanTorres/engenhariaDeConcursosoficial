using Domain.Entities;

namespace Application.DTOs.QLevel;

public class UpdateQLevelDto
{
  public required Guid Id { get; set; }
  public string? Name { get; set; }
  public string? About { get; set; }
}
