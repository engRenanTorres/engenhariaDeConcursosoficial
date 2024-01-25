using Apllication.Exceptions.VO;
using Application.Exceptions;

namespace Apllication.Exceptions
{
  public class BadRequestException : WebException
  {
    public override WebExceptionVO ErrorDetails { get; set; } = new WebBadRequestVO();

    public BadRequestException(string message)
      : base(message) { }

    public BadRequestException(string message, WebBadRequestVO errorDetails)
      : base(message)
    {
      this.ErrorDetails = errorDetails;
    }
  }
}
