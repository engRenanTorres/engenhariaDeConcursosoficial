using Domain.Entities;
using Domain.Entities.Inharitance;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Data;
public class DataContextEF : DbContext
{

  public DataContextEF(DbContextOptions<DataContextEF> options) : base(options)
  {
    // _conectionString = config.GetConnectionString("DefaultConnection");
  }
  //public DbSet<User>? Users { get; set; }
  public DbSet<BaseQuestion> Questions { get; set; }
  public DbSet<BooleanQuestion> BooleanQuestions { get; set; }
  public DbSet<MultipleChoicesQuestion> MultipleChoiceQuestions { get; set; }
  public DbSet<Choice> Choices { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    /*modelBuilder.Entity<User>()
      .HasMany(q => q.Questions)
      .WithOne(u => u.CreatedBy)
      .IsRequired();*/
    // To use Table Per Type TPT:
    // modelBuilder.Entity<MultipleChoicesQuestion>().ToTable("Muliple_Choice_Questions");
    // modelBuilder.Entity<BooleanQuestion>().ToTable("Boolean_Questions");
  }
}
