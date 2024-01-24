//using Microsoft.AspNetCore.Mvc.ModelBinding;
//using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
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
}
