namespace Domain.Entities.Questions;

public class ChoicesQuestion : Question
{
  public char Answer { get; set; }
  public ICollection<Choice>? Choices { get; set; }
}
