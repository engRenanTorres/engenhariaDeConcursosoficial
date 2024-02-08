using Apllication.DTOs;
using Domain.Entities;
using Domain.Entities.Inharitance;

namespace Application.DTOs.Institute;

public class ViewInstituteDto
{
  public Guid Id { get; set; }
  public string? Name { get; set; }
  public string? About { get; set; }
}
