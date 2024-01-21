using System.Net;

namespace Apllication.Exceptions.VO
{
  public class DBExceptionVO : WebExceptionVO
  {
    public override string? Code { get; set; } = "Database Exception!";
    public override HttpStatusCode HttpStatus { get; set; } = HttpStatusCode.InternalServerError;
  }
}
