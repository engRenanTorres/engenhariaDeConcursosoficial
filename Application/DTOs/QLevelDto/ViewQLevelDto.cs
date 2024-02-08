using Apllication.DTOs;
using Domain.Entities;
using Domain.Entities.Inharitance;

namespace Application.DTOs.QLevel;

public class ViewQLevelDto
{
  public Guid Id { get; set; }
  public string? Name { get; set; }
  public string? About { get; set; }
}
