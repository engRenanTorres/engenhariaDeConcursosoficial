using System.Text;
using Apllication.Services;
using Domain.Entities;
using Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
    services.AddAuthorization(opt =>
    {
      opt.AddPolicy(
        "IsActivityHost",
        policy =>
        {
          policy.Requirements.Add(new IsHostRequirement());
        }
      );
    });
    services.AddTransient<IAuthorizationHandler, IsHostRequirementHandler>();
    services.AddScoped<IAuthService, AuthService>();

    return services;
  }
}
