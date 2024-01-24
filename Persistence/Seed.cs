using Domain.Entities;
using Domain.Entities.Inharitance;
using Microsoft.AspNetCore.Identity;
using Persistence.Data;

namespace Persistence.Seed;

public class Seed
{
  public static async Task SeedData(DataContext contextEF, UserManager<AppUser> userManager)
  {
    Console.WriteLine("!userManager.Users.Any() = " + !userManager.Users.Any());
    if (!userManager.Users.Any())
    {
      Console.WriteLine(
        "----------------------------------------------------------------------fui chamado1"
      );
      var users = new List<AppUser>()
      {
        new()
        {
          DisplayName = "Admin",
          UserName = "adm",
          Email = "adm@test.com.br"
        },
        new()
        {
          DisplayName = "Staff",
          UserName = "sta",
          Email = "st@test.com.br"
        }
      };

      foreach (var user in users)
      {
        var x = await userManager.CreateAsync(user, "Pa$$word");
        Console.WriteLine("foi adicionado? = " + x);
      }
    }

    if (contextEF.BaseQuestions.Any())
      return;

    var choiceA = new Choice() { Letter = 'A', Text = "Jéssica" };
    var choiceB = new Choice() { Letter = 'B', Text = "Renan" };
    var choiceC = new Choice() { Letter = 'C', Text = "Alfredo" };
    var choiceD = new Choice() { Letter = 'D', Text = "Alberto" };

    var questions = new List<BooleanQuestion>
    {
      new()
      {
        Answer = 'A',
        Body = "Renan é foda!",
        CreatedAt = DateTime.UtcNow,
        LastUpdatedAt = DateTime.UtcNow,
        CreatedById = 1,
      },
      new()
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
      new()
      {
        Answer = 'B',
        Body = "Quem é o mais legal?",
        CreatedAt = DateTime.UtcNow,
        LastUpdatedAt = DateTime.UtcNow,
        CreatedById = 1,
        Choices = new List<Choice>() { choiceA, choiceB, choiceC, choiceD }
      }
    };
    await contextEF.BooleanQuestions.AddRangeAsync(questions);
    await contextEF.MultipleChoicesQuestions.AddRangeAsync(questionsMultipleChoice);
    await contextEF.SaveChangesAsync();
  }
}
