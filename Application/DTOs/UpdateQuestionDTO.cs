using Domain.Entities;

namespace Application.DTOs;

public class UpdateQuestionDTO
{
  public string? Body { get; set; }
  public char? Answer { get; set; }
  public string? Tip { get; set; }
  public ICollection<ChoiceDTO>? Choices { get; set; }
}
