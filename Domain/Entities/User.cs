using Domain.Entities.Inharitance;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

//[Index(nameof(User.Email), IsUnique = true)]
public class AppUser : IdentityUser
{
  public required string DisplayName { get; set; }
  public string Bio { get; set; } = "";
  public Roles Role { get; set; } = Roles.User;
  public ICollection<Question?> Questions { get; set; } = [];
}
