using System.Net;

namespace Apllication.Exceptions.VO
{
  public class AiErrorVO : WebExceptionVO
  {
    public override string? Code { get; set; } = "Something went worng with External API!";
    public override HttpStatusCode HttpStatus { get; set; } = HttpStatusCode.FailedDependency;
  }
}
