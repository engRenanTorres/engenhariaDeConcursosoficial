using System.Net;

namespace Apllication.Exceptions.VO
{
  public class WebExceptionVO
  {
    public string? Type { get; set; } = "";
    public string? Param { get; set; } = "";
    public virtual string? Code { get; set; } = "Internal Error";
    public virtual HttpStatusCode HttpStatus { get; set; } = HttpStatusCode.InternalServerError;
  }
}
