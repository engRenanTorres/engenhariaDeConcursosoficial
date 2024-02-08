namespace Domain.Entities.Interfaces;

public interface IEntity
{
  Guid Id { get; set; }
  
  public DateTime CreatedAt { get; set; }
}
