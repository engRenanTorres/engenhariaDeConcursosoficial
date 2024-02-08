using System.ComponentModel.DataAnnotations;
using Application.DTOs;

namespace Apllication.DTOs.Concurso;

public class CreateConcursoDto
{
  [Required]
  public required string Name { get; set; } = "";
  public string About { get; set; } = "";

  [Required, Range(2010, 2030)]
  public int Year { get; set; }

  [Required]
  public Guid InstituteId { get; set; }
}
