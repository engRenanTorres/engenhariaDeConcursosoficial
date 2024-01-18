using Domain.Entities;

namespace API.DTOs;

public class UpdateQuestionDTO
{
    public string? Body { get; set; }
    public char? Answer { get; set; }
    public string? Tip { get; set; }
    public ICollection<Choice>? Choices { get; set; }
}
