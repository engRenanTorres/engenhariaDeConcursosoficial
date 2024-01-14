
using Domain.Entities.Inharitance;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;
public class BooleanQuestion : BaseQuestion
{
    private Choice _ChoiceA = new()
    {
        Letter = 'A',
        Text = "Verdadeiro",
    };
    private Choice _ChoiceB = new()
    {
        Letter = 'B',
        Text = "Falso"
    };

    public override ICollection<Choice?> Choices
    {
        get => new[] { _ChoiceA, _ChoiceB };
    }
}