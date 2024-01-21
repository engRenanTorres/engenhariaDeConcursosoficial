using Apllication.Exceptions.VO;

namespace Application.Errors.Exceptions
{
  public class ExternalApiException : Exception
  {
    public AiErrorVO ErrorDetails { get; set; }

    public ExternalApiException(string message, AiErrorVO errorDetails)
      : base(message)
    {
      this.ErrorDetails = errorDetails;
    }
  }
}
