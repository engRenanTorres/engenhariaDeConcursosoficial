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

  //public DbSet<User>? Users { get; set; }
  public DbSet<BaseQuestion> BaseQuestions { get; set; }
  public DbSet<BooleanQuestion> BooleanQuestions { get; set; }
  public DbSet<MultipleChoicesQuestion> MultipleChoicesQuestions { get; set; }
  public DbSet<Choice> Choices { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.Entity<AppUser>().HasKey(x => x.Id);
    /*modelBuilder.Entity<User>()
      .HasMany(q => q.Questions)
      .WithOne(u => u.CreatedBy)
      .IsRequired();*/
    modelBuilder
      .Entity<MultipleChoicesQuestion>()
      .HasMany(x => x.Choices)
      .WithOne()
      .OnDelete(DeleteBehavior.Cascade);
    // To use Table Per Type TPT:
    // modelBuilder.Entity<MultipleChoicesQuestion>().ToTable("Muliple_Choice_Questions");
    // modelBuilder.Entity<BooleanQuestion>().ToTable("Boolean_Questions");
  }
}
