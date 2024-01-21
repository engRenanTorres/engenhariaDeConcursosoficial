using System.Net;

namespace Apllication.Exceptions.VO
{
  public class WebNotFoundVO : WebExceptionVO
  {
    public override string? Code { get; set; } = "Not Found Exception!";
    public override HttpStatusCode HttpStatus { get; set; } = HttpStatusCode.NotFound;
  }
}
