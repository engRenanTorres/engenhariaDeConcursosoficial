using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Inharitance;

namespace Domain.Entities;

public class BooleanQuestion : BaseQuestion
{
    public BooleanQuestion()
    {
        _choices = new List<Choice>
        {
            new() { Letter = 'A', Text = "Verdadeiro" },
            new() { Letter = 'B', Text = "Falso" }
        };
    }

    public override ICollection<Choice> Choices
    {
        get => _choices;
        set =>
            _choices = new List<Choice>
            {
                new() { Letter = 'A', Text = "Verdadeiro" },
                new() { Letter = 'B', Text = "Falso" }
            };
    }
}
