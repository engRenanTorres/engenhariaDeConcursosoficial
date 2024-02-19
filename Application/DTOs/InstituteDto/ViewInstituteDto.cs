using Apllication.DTOs;
using Domain.Entities;
using Domain.Entities.Questions;

namespace Application.DTOs.Institute;

public class ViewInstituteDto
{
  public Guid Id { get; set; }
  public string? Name { get; set; }
  public DateTime? CreatedAt { get; set; }
  public string? About { get; set; }
}
