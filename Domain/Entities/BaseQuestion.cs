using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Inharitance;

[Table("Questions")]
public abstract class BaseQuestion
{
  [Key]
  public int Id { get; set; }

  [Column(name: "Created_at")]
  public DateTime CreatedAt { get; set; }

  [Column(name: "Last_updated_at")]
  public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;
  public string Body { get; set; } = "";
  public char Answer { get; set; }
  public string Tip { get; set; } = "";
  public required QuestionLevel QuestionLevel { get; set; }
  public required Concurso Concurso { get; set; }
  public required Subject Subject { get; set; }
  protected ICollection<Choice> _choices = new List<Choice>();
  public AppUser? CreatedBy { get; set; }
  public AppUser? EditedBy { get; set; }
  public abstract ICollection<Choice> Choices { get; set; }
}
