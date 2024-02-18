using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Inharitance;
using Domain.Entities.Interfaces;

namespace Domain.Entities;

public class Concurso : IEntity
{
  [Key]
  public Guid Id { get; set; }

  [Column(name: "Created_at")]
  public DateTime CreatedAt { get; set; }

  public string Name { get; set; } = "";

  public string About { get; set; } = "";

  public int Year { get; set; }

  public required Institute Institute { get; set; }

  public IEnumerable<Question> Questions { get; set; } = new List<Question>();
}
