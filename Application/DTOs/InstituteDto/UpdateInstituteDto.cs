using Domain.Entities;

namespace Application.DTOs.Institute;

public class UpdateInstituteDto
{
  public string? Name { get; set; }
  public string? About { get; set; }
  public string? Contact { get; set; }
}
