using Apllication.Exceptions.VO;

namespace Application.Exceptions
{
  public class WebException : Exception
  {
    public virtual WebExceptionVO ErrorDetails { get; set; } = new WebExceptionVO();

    public WebException(string? message)
      : base(message) { }
  }
}
