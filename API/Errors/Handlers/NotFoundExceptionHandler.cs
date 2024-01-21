using System.Net;
using API.Errors;
using Microsoft.AspNetCore.Diagnostics;

namespace API.Errors.Handler;

public class NotFoundExceptionHandler : IExceptionHandler
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
      var response = new ErrorResponse()
      {
        StatusCode = (int)((ex?.ErrorDetails?.HttpStatus ?? HttpStatusCode.InternalServerError)),
        Title = ex?.ErrorDetails?.Code ?? "WebException Error",
        ExceptionMessage = ex?.ErrorDetails.Message
      };
      await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
      httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
      return true;
    }
    return false;
  }
}
