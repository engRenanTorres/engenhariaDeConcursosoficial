using Apllication.Exceptions.VO;
using Application.Exceptions;

namespace Application.Errors.Exceptions
{
  public class DatabaseException : WebException
  {
    public override WebExceptionVO ErrorDetails { get; set; } = new DBExceptionVO();

    public DatabaseException(string message, AiErrorVO errorDetails)
      : base(message)
    {
      this.ErrorDetails = errorDetails;
    }

    public DatabaseException(string message)
      : base(message) { }
  }
}
