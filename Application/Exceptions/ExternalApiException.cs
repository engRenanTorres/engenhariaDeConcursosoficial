using Apllication.Exceptions.VO;
using Application.Exceptions;

namespace Application.Errors.Exceptions
{
  public class ExternalApiException : WebException
  {
    public override WebExceptionVO ErrorDetails { get; set; } = new AiErrorVO();

    public ExternalApiException(string message, AiErrorVO errorDetails)
      : base(message)
    {
      this.ErrorDetails = errorDetails;
    }

    public ExternalApiException(string message)
      : base(message) { }
  }
}
