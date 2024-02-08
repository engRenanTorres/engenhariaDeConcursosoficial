using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Interfaces;

namespace Domain.Entities;

public class Institute : IEntity
{
  [Key]
  public Guid Id { get; set; }

  [Column(name: "Created_at")]
  public DateTime CreatedAt { get; set; }

  public string Name { get; set; } = "";

  public string About { get; set; } = "";

  public string Contact { get; set; } = "";

  public IEnumerable<Concurso> Concursos { get; set; } = new List<Concurso>();
}
