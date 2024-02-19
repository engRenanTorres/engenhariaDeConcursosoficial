using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Interfaces;
using Domain.Entities.Questions;

namespace Domain.Entities;

public class QuestionLevel : IEntity
{
  [Key]
  public Guid Id { get; set; }

  [Column(name: "Created_at")]
  public DateTime CreatedAt { get; set; }
  public string Name { get; set; } = "";
  public string About { get; set; } = "";
  public IEnumerable<Question> Questions { get; set; } = new List<Question>();
}
