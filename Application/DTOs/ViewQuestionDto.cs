using Apllication.DTOs;
using Domain.Entities;

namespace Application.DTOs;

public class ViewQuestionDto
{
  public int Id { get; set; }
  public char Answer { get; set; }
  public string Body { get; set; } = "";

  public ICollection<Choice> Choices { get; set; } = [];
  public DateTime CreatedAt { get; set; }
  public UserDto? CreatedBy { get; set; }

  public UserDto? EditedBy { get; set; }
  public DateTime LastUpdatedAt { get; set; }

  public string? Tip { get; set; } = "";
}
