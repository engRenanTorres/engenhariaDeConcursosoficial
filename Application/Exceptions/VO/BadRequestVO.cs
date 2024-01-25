using System.Net;

namespace Apllication.Exceptions.VO
{
  public class WebBadRequestVO : WebExceptionVO
  {
    public override string? Code { get; set; } = "Bad Request Exception!";
    public string? Errors { get; set; }
    public override HttpStatusCode HttpStatus { get; set; } = HttpStatusCode.BadRequest;
  }
}
