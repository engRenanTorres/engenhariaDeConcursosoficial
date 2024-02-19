using Apllication.DTOs;
using Domain.Entities;
using Domain.Entities.Questions;

namespace Application.DTOs.QLevel;

public class ViewQLevelDto
{
  public Guid Id { get; set; }
  public string? Name { get; set; }
  public DateTime? CreatedAt { get; set; }
  public string? About { get; set; }
}
