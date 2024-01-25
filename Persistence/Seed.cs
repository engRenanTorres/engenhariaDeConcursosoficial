using Domain.Entities;
using Domain.Entities.Inharitance;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Persistence.Seed;

public class Seed
{
  public static async Task SeedData(
    DataContext contextEF,
    UserManager<AppUser> userManager,
    RoleManager<IdentityRole> roleManager
  )
  {
    if (!await roleManager.Roles.AnyAsync())
    {
      var roles = new List<string>() { "Admin", "Manager", "Member", "User" };
      foreach (var role in roles)
      {
        await roleManager.CreateAsync(new IdentityRole(role));
      }
    }

    if (!await userManager.Users.AnyAsync())
    {
      Console.WriteLine("Creating Default users");
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
          DisplayName = "User",
          UserName = "user",
          Email = "user@test.com.br"
        }
      };

      foreach (var user in users)
      {
        var result = await userManager.CreateAsync(user, "Senhazo1!");
        Console.WriteLine("foi adicionado? = " + result);
      }
      await userManager.AddToRoleAsync(users[0], "Admin");
      await userManager.AddToRoleAsync(users[1], "User");
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
