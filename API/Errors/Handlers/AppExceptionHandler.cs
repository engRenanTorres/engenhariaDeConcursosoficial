using System.Net;
using Microsoft.AspNetCore.Diagnostics;

namespace API.Errors.Handler;

public class AppExceptionHandler : IExceptionHandler
{
  public async ValueTask<bool> TryHandleAsync(
    HttpContext httpContext,
    Exception exception,
    CancellationToken cancellationToken
  )
  {
    if (exception is Application.Exceptions.WebException)
    {
      var ex = exception as Application.Exceptions.WebException;
      var statusCode = ex?.ErrorDetails?.HttpStatus ?? HttpStatusCode.InternalServerError;
      var response = new ErrorResponse()
      {
        StatusCode = (int)statusCode,
        Title = ex?.ErrorDetails?.Code ?? "WebException Error",
        ExceptionMessage = ex?.Message,
        StackTrace = exception?.StackTrace?.ToString()
      };
      await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
      httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
      return true;
    }
    else
    {
      var response = new ErrorResponse()
      {
        StatusCode = StatusCodes.Status500InternalServerError,
        Title = "Something went wrong!",
        ExceptionMessage = exception.Message,
        StackTrace = exception?.StackTrace?.ToString()
      };
      await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
      httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
      return true;
    }
  }
}
