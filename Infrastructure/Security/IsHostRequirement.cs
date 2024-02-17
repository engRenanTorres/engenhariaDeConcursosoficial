using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Persistence.Data;

namespace Infrastructure.Security
{
  public class IsHostRequirement : IAuthorizationRequirement { }

  public class IsHostRequirementHandler : AuthorizationHandler<IsHostRequirement>
  {
    private readonly DataContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public IsHostRequirementHandler(DataContext context, IHttpContextAccessor httpContextAccessor)
    {
      _context = context;
      _httpContextAccessor = httpContextAccessor;
    }

    protected override Task HandleRequirementAsync(
      AuthorizationHandlerContext context,
      IsHostRequirement requirement
    )
    {
      var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

      if (userId == null)
        return Task.CompletedTask;

      var routeData = _httpContextAccessor.HttpContext?.GetRouteData();

      if (routeData == null)
        return Task.CompletedTask;

      // inserir policy aqui

      if (routeData.Values.TryGetValue("id", out var idValue))
      {
        if (Guid.TryParse(idValue.ToString(), out var activityId))
        {
          if (idValue.ToString() == userId)
          {
            context.Succeed(requirement);
          }
        }
      }

      return Task.CompletedTask;
    }
  }
}
