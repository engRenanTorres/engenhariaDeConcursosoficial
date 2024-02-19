using Apllication.DTOs;
using Domain.Entities;
using Domain.Entities.Questions;

namespace Application.DTOs.Concurso;

public class ViewConcursoDto
{
  public required string Name { get; set; }
  public string? About { get; set; }

  public int? Year { get; set; }

  public string? InstituteName { get; set; }
}
