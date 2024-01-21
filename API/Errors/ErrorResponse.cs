using Microsoft.AspNetCore.Diagnostics;

namespace API.Errors;

public class ErrorResponse
{
  public int StatusCode { get; set; }
  public required string Title { get; set; }

  public string? ExceptionMessage { get; set; }
}
