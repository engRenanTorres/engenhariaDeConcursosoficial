using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Questions;

[Table("Questions")]
public abstract class Question
{
  [Key]
  public int Id { get; set; }
  public DateTime InsertedAt { get; set; }
  public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;
  public string Body { get; set; } = "";
  public string Tip { get; set; } = "";
  public required QuestionLevel QuestionLevel { get; set; }
  public required Concurso Concurso { get; set; }
  public required Subject Subject { get; set; }
  public AppUser? InsertedBy { get; set; }
  public AppUser? EditedBy { get; set; }
}
