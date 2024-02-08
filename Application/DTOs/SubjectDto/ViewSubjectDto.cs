namespace Application.DTOs.SubjectDto;

public class ViewSubjectDto
{
  public Guid Id { get; set; }
  public string? Name { get; set; }
  public string? About { get; set; }
  public Domain.Entities.StudyArea? Area { get; set; }
}
