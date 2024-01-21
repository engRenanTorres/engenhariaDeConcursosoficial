using API.Errors;
using Apllication.Core;
using Apllication.Repositories;
using Apllication.Services;
using Apllication.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Persistence.Data.Repositories;
using Persistence.Seed;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder
  .Services.AddEntityFrameworkNpgsql()
  .AddDbContext<DataContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
  );

builder.Services.AddCors(
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

//builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

//Services
//builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IQuestionService, QuestionService>();

//builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);
builder.Services.AddExceptionHandler<AppExceptionHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseExceptionHandler(_ => { });

app.UseAuthorization();

app.MapControllers();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
  var context = services.GetRequiredService<DataContext>();
  await context.Database.MigrateAsync();
  await Seed.SeedData(context);
}
catch (Exception ex)
{
  var logger = services.GetRequiredService<ILogger<Program>>();
  logger.LogError(ex, "An Error ocurred during migration");
}

app.Run();
