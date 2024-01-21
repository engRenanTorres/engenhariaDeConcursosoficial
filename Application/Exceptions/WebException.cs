using Apllication.Exceptions.VO;

namespace Application.Exceptions
{
  public abstract class WebException : Exception
  {
    public abstract WebExceptionVO ErrorDetails { get; set; }

    public WebException(string? message)
      : base(message) { }
  }
}
