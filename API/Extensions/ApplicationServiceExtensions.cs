using Apllication.Core;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace API.Extensions;

public static class ApplicationServiceExtensions
{
  public static IServiceCollection AddApplicationServices(
    this IServiceCollection services,
    IConfiguration configuration
  )
  {
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();

    services
      .AddEntityFrameworkNpgsql()
      .AddDbContext<DataContext>(options =>
        options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
      );

    services.AddAutoMapper(typeof(MappingProfile).Assembly);
    services.AddExceptionHandler<API.Errors.Handler.AppExceptionHandler>();

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
              .WithOrigins("https://engenhariadeconcursos.com.br")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
          }
        );
      }
    );
    return services;
  }
}
