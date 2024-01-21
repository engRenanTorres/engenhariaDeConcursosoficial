using Apllication.Exceptions.VO;
using Application.Exceptions;

namespace Apllication.Exceptions
{
  public class NotFoundException : WebException
  {
    public override WebExceptionVO ErrorDetails { get; set; } = new WebNotFoundVO();

    public NotFoundException(string message)
      : base(message) { }

    public NotFoundException(string message, WebNotFoundVO errorDetails)
      : base(message)
    {
      this.ErrorDetails = errorDetails;
    }
  }
}
