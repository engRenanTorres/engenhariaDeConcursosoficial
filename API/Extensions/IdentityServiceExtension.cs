using System.Text;
using Apllication.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
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
        opt.Password.RequiredLength = 6;
        opt.Password.RequireNonAlphanumeric = true;
        opt.User.RequireUniqueEmail = true;
      })
      .AddRoles<IdentityRole>()
      .AddEntityFrameworkStores<DataContext>();

    var key = new SymmetricSecurityKey(
      Encoding.UTF8.GetBytes(configuration.GetSection("AppSettings:PasswordKey").Value + "salt")
    );

    services
      .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
      .AddJwtBearer(opt =>
      {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = key,
          ValidateIssuer = false,
          ValidateAudience = false
        };
      });
    services.AddScoped<IAuthService, AuthService>();

    return services;
  }
}
