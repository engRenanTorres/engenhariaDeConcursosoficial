using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

public class ChoiceDTO
{
  [Required]
  [StringLength(1)]
  [RegularExpression(
    @"^[A|B|C|D|E]$",
    ErrorMessage = "Answer field accepts only the values A, B, C, D or E."
  )]
  public string Letter { get; set; } = "A";

  [Required]
  public string Text { get; set; } = "";
}
