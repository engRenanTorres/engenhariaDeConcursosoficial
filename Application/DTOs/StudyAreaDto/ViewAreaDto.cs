using Apllication.DTOs;
using Domain.Entities;
using Domain.Entities.Inharitance;

namespace Application.DTOs.StudyArea;

public class ViewAreaDto
{
  public Guid Id { get; set; }
  public string? Name { get; set; }
  public string? About { get; set; }
}
