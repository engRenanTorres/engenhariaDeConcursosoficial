using Domain.Entities;
using Domain.Entities.Inharitance;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Data;

public class DataContext : IdentityDbContext<AppUser>
{
  public DataContext(DbContextOptions<DataContext> options)
    : base(options)
  {
    // _conectionString = config.GetConnectionString("DefaultConnection");
  }

  public DbSet<Question> BaseQuestions { get; set; }
  public DbSet<Choice> Choices { get; set; }
  public DbSet<Concurso> Concursos { get; set; }
  public DbSet<Institute> Institutes { get; set; }
  public DbSet<QuestionLevel> QuestionLevels { get; set; }
  public DbSet<Subject> Subjects { get; set; }
  public DbSet<StudyArea> StudyAreas { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.Entity<AppUser>().HasKey(u => u.Id);
    modelBuilder.Entity<Question>().HasOne(x => x.CreatedBy).WithMany(x => x.Questions);
    modelBuilder
      .Entity<Institute>()
      .HasMany(x => x.Concursos)
      .WithOne(x => x.Institute)
      .OnDelete(DeleteBehavior.Cascade);
    modelBuilder
      .Entity<Concurso>()
      .HasMany(x => x.Questions)
      .WithOne(x => x.Concurso)
      .OnDelete(DeleteBehavior.Cascade);
    modelBuilder
      .Entity<Question>()
      .HasMany(x => x.Choices)
      .WithOne()
      .OnDelete(DeleteBehavior.Cascade);
    // To use Table Per Type TPT:
    // modelBuilder.Entity<MultipleChoicesQuestion>().ToTable("Muliple_Choice_Questions");
    // modelBuilder.Entity<BooleanQuestion>().ToTable("Boolean_Questions");
  }
}
