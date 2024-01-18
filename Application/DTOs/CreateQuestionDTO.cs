using System.ComponentModel.DataAnnotations;
using Application.DTOs;

namespace Apllication.DTOs;

public class CreateQuestionDTO
{
  [Required]
  public string Body { get; set; } = "";

  [Required]
  [StringLength(1)]
  [RegularExpression(
    @"^[A|B|C|D|E]$",
    ErrorMessage = "Answer field accepts only the values A, B, C, D, or E."
  )]
  public string Answer { get; set; } = "A";

  public string? Tip { get; set; }
  public ICollection<ChoiceDTO>? Choices { get; set; }
}
