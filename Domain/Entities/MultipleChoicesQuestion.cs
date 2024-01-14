namespace Domain.Entities;

using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Inharitance;

public class MultipleChoicesQuestion : BaseQuestion
{
    public required Choice ChoiceA { get; set; }
    public required Choice ChoiceB { get; set; }

    public required Choice? ChoiceC { get; set; }
    public required Choice? ChoiceD { get; set; }

    public override ICollection<Choice?> Choices
    {
        get => new[] { ChoiceA, ChoiceB, ChoiceC, ChoiceD };
    }
}
