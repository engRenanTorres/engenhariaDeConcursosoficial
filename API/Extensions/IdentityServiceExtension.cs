using Domain.Entities;
using Persistence.Data;

namespace API.Extensions;

public static class IdentityServiceExtensions
{
  public static IServiceCollection AddIdentityServices(
    this IServiceCollection services,
    IConfiguration configuration
  )
  {
    services
      .AddIdentityCore<AppUser>(opt =>
      {
        //opt.Password.RequiredLength = 6;
        opt.Password.RequireNonAlphanumeric = false;
      })
      .AddEntityFrameworkStores<DataContext>();

    services.AddAuthentication();

    return services;
  }
}
