using System.ComponentModel.DataAnnotations;
using Domain.Entities;

namespace Application.DTOs.Concurso;

public class UpdateConcursoDto
{
  public required string Name { get; set; }
  public string? About { get; set; }

  [Range(2010, 2030)]
  public int? Year { get; set; }

  public Guid? InstituteId { get; set; }
}
