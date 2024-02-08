using Domain.Entities;

namespace Application.DTOs.SubjectDto;

public class UpdateSubjectDto
{
  public string? Name { get; set; }
  public Guid? AreaId { get; set; }
  public string? About { get; set; }
}
