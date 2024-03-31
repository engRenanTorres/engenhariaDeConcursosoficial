using Apllication.Core;
using Apllication.Interfaces;
using Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Persistence.Data;

namespace API.Extensions;

public static class ApplicationServiceExtensions
{
  public static IServiceCollection AddApplicationServices(
    this IServiceCollection services,
    IConfiguration configuration
  )
  {
    services.AddCors(
      (options) =>
      {
        options.AddPolicy(
          "DevCors",
          (corsBuilder) =>
          {
            corsBuilder
              .WithOrigins("http://localhost:3000")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
          }
        );
        options.AddPolicy(
          "ProdCors",
          (corsBuilder) =>
          {
            corsBuilder
              .WithOrigins("http://localhost:3000")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
          }
        );
      }
    );
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen(options =>
    {
      options.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
          Scheme = "bearer",
          BearerFormat = "JWT",
          In = ParameterLocation.Header,
          Name = "Authorization",
          Description = "Bearer Authentication with JWT Token",
          Type = SecuritySchemeType.Http
        }
      );
      options.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
          {
            new OpenApiSecurityScheme
            {
              Reference = new OpenApiReference
              {
                Id = "Bearer",
                Type = ReferenceType.SecurityScheme
              }
            },
            new List<string>()
          }
        }
      );
    });

    services
      .AddEntityFrameworkNpgsql()
      .AddDbContext<DataContext>(options =>
        options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
      );

    // TODO: Use Mapster and remove AutoMapper
    services.AddAutoMapper(typeof(MappingProfile).Assembly);
    services.AddExceptionHandler<API.Errors.Handler.AppExceptionHandler>();
    services.AddHttpContextAccessor();
    services.AddScoped<IUserAccessor, UserAccessor>();

    return services;
  }
}
