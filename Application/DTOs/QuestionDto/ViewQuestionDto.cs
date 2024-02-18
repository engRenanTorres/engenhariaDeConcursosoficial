using Apllication.DTOs;
using Application.DTOs.Concurso;
using Application.DTOs.QLevel;
using Application.DTOs.SubjectDto;
using Domain.Entities;

namespace Application.DTOs;

public class ViewQuestionDto
{
  public int Id { get; set; }
  public char Answer { get; set; }
  public string Body { get; set; } = "";

  public required ViewConcursoDto Concurso { get; set; }
  public required string Subject { get; set; }
  public required string StudyArea { get; set; }

  public required string Level { get; set; }

  public ICollection<Choice> Choices { get; set; } = [];
  public DateTime CreatedAt { get; set; }
  public UserDto? CreatedBy { get; set; }

  public UserDto? EditedBy { get; set; }
  public DateTime LastUpdatedAt { get; set; }

  public string? Tip { get; set; } = "";
}
