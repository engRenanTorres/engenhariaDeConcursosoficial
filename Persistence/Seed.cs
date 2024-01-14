using Domain.Entities;
using Domain.Entities.Inharitance;
using Persistence.Data;

namespace Persistence.Seed;
public class Seed
{
  public static async Task SeedData(DataContext contextEF)
  {
    if (contextEF.Questions.Any()) return;

    var choiceA = new Choice()
    {
      Letter = 'A',
      Text = "Jéssica"
    };
    var choiceB = new Choice()
    {
      Letter = 'B',
      Text = "Renan"
    };
    var choiceC = new Choice()
    {
      Letter = 'C',
      Text = "Alfredo"
    };
    var choiceD = new Choice()
    {
      Letter = 'D',
      Text = "Alberto"
    };

    var questions = new List<BooleanQuestion>
    {
      new ()
      {
        Answer = 'A',
        Body = "Renan é foda!",
        CreatedAt = DateTime.UtcNow,
        LastUpdatedAt = DateTime.UtcNow,
        CreatedById = 1,
      },
      new ()
      {
        Answer = 'B',
        Body = "Renan é chato!",
        CreatedAt = DateTime.UtcNow,
        LastUpdatedAt = DateTime.UtcNow,
        CreatedById = 1,
      },
    };
    var questionsMultipleChoice = new List<MultipleChoicesQuestion>
    {
      new () {
        Answer = 'B',
        Body = "Quem é o mais legal?",
        CreatedAt = DateTime.UtcNow,
        LastUpdatedAt = DateTime.UtcNow,
        CreatedById = 1,
        ChoiceA = choiceA,
        ChoiceB = choiceB,
        ChoiceC = choiceC,
        ChoiceD = choiceD,
      }
    };

    if (!contextEF.Choices.Any())
    {
      await contextEF.Choices.AddRangeAsync([choiceA, choiceB, choiceC, choiceD]);
    }
    await contextEF.BooleanQuestions.AddRangeAsync(questions);
    await contextEF.MultipleChoiceQuestions.AddRangeAsync(questionsMultipleChoice);
    await contextEF.SaveChangesAsync();
  }

}