using API.Errors;
using API.Extensions;
using Apllication.Core;
using Apllication.Data.Repositories;
using Apllication.Repositories;
using Apllication.Repositories.Interfaces;
using Apllication.Services;
using Apllication.Services.Interfaces;
using Application.Data.Repositories;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Persistence.Data;
using Persistence.Seed;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(opt =>
{
  var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
  opt.Filters.Add(new AuthorizeFilter(policy));
});
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

//Repos
builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
builder.Services.AddScoped<IQuestionLevelRepository, QuestionLevelRepository>();
builder.Services.AddScoped<ISubjectRepository, SubjectRepository>();
builder.Services.AddScoped<IInstituteRepository, InstituteRepository>();
builder.Services.AddScoped<IConcursoRepository, ConcursoRepository>();
builder.Services.AddScoped<IStudyAreaRepository, StudyAreaRepository>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

//Services
//builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<IQLevelService, QLevelService>();
builder.Services.AddScoped<ISubjectService, SubjectService>();
builder.Services.AddScoped<IInstituteService, InstituteService>();
builder.Services.AddScoped<IConcursoService, ConcursoService>();
builder.Services.AddScoped<IAreaService, AreaService>();

//builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseStaticFiles(
  new StaticFileOptions
  {
    FileProvider = new PhysicalFileProvider(
      Path.Combine(Directory.GetCurrentDirectory(), "static")
    ),
    RequestPath = "/static"
  }
);

//app.UseHttpsRedirection();
app.UseExceptionHandler(_ => { });

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
  var context = services.GetRequiredService<DataContext>();
  var useManager = services.GetRequiredService<UserManager<AppUser>>();
  var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
  await context.Database.MigrateAsync();
  await Seed.SeedData(context, useManager, roleManager);
}
catch (Exception ex)
{
  var logger = services.GetRequiredService<ILogger<Program>>();
  logger.LogError(ex, "An Error ocurred during migration");
}

app.Run();
