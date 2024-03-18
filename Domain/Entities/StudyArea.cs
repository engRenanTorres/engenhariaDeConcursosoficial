using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Domain.Entities.Interfaces;
using Domain.Entities.Questions;

namespace Domain.Entities;

public class StudyArea : IEntity
{
  [Key]
  public Guid Id { get; set; }

  [Column(name: "Created_at")]
  public DateTime CreatedAt { get; set; }

  public string Name { get; set; } = "";

  public string About { get; set; } = "";

  [JsonIgnore]
  public IEnumerable<Subject> Subjects { get; set; } = new List<Subject>();
}
