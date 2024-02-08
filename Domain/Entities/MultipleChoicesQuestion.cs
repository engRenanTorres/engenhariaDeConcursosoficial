using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Inharitance;

namespace Domain.Entities;

public class MultipleChoicesQuestion : BaseQuestion
{
  public override ICollection<Choice> Choices
  {
    get => _choices;
    set => _choices = value;
  }
}
