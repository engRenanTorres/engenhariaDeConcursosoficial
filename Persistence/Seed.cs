using Domain.Entities;
using Domain.Entities.Inharitance;
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
    AppUser? _creator;
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
      _creator = users[0];
      foreach (var user in users)
      {
        var result = await userManager.CreateAsync(user, "Senhazo1!");
        Console.WriteLine("foi adicionado? = " + result);
      }
      await userManager.AddToRoleAsync(users[0], "Admin");
      await userManager.AddToRoleAsync(users[1], "User");

      var level1 = new QuestionLevel { Name = "Superior", CreatedAt = DateTime.UtcNow, };
      if (!contextEF.QuestionLevels.Any())
      {
        await contextEF.QuestionLevels.AddAsync(level1);
        await contextEF.SaveChangesAsync();
      }

      var institute = new Institute { Name = "Cebraspe", CreatedAt = DateTime.UtcNow, };

      var concurso1 = new Concurso
      {
        Name = "Petrobras",
        Year = 2010,
        Institute = institute,
        CreatedAt = DateTime.UtcNow,
      };
      if (!contextEF.Institutes.Any())
      {
        await contextEF.Institutes.AddAsync(institute);
        await contextEF.Concursos.AddAsync(concurso1);
        await contextEF.SaveChangesAsync();
      }

      StudyArea studyArea = new() { Name = "Engenharia civil", CreatedAt = DateTime.UtcNow, };
      Subject subject =
        new()
        {
          Name = "Resistência dos Materiais",
          StudyArea = studyArea,
          CreatedAt = DateTime.UtcNow,
        };
      if (!contextEF.StudyAreas.Any())
      {
        await contextEF.StudyAreas.AddAsync(studyArea);
        await contextEF.Subjects.AddAsync(subject);
        await contextEF.SaveChangesAsync();
      }

      if (contextEF.BaseQuestions.Any())
        return;

      var choiceA = new Choice() { Letter = 'A', Text = "Jéssica" };
      var choiceB = new Choice() { Letter = 'B', Text = "Renan" };
      var choiceC = new Choice() { Letter = 'C', Text = "Alfredo" };
      var choiceD = new Choice() { Letter = 'D', Text = "Alberto" };

      var questions = new List<Question>
      {
        new()
        {
          Answer = 'A',
          Body = "Renan é foda!",
          CreatedAt = DateTime.UtcNow,
          CreatedBy = _creator,
          QuestionLevel = level1,
          Concurso = concurso1,
          Subject = subject,
        },
        new()
        {
          Answer = 'B',
          Body = "Renan é chato!",
          CreatedAt = DateTime.UtcNow,
          CreatedBy = _creator,
          QuestionLevel = level1,
          Concurso = concurso1,
          Subject = subject,
        },
      };

      var questionsMultipleChoice = new List<Question>
      {
        new()
        {
          Answer = 'B',
          Body = "Quem é o mais legal?",
          CreatedAt = DateTime.UtcNow,
          LastUpdatedAt = DateTime.UtcNow,
          CreatedBy = _creator,
          QuestionLevel = level1,
          Concurso = concurso1,
          Subject = subject,
          Choices = new List<Choice>() { choiceA, choiceB, choiceC, choiceD }
        }
      };
      await contextEF.BaseQuestions.AddRangeAsync(questions);
      await contextEF.BaseQuestions.AddRangeAsync(questionsMultipleChoice);
      await contextEF.SaveChangesAsync();
    }
  }
}
