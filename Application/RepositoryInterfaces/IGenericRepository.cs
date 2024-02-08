namespace Apllication.Repositories.Interfaces;

public interface IGenericRepository<T>
  where T : class
{
  public Task<bool> SaveChanges();
  public void Add(T entity);
  public void Remove(T entity);
  public Task<IEnumerable<T?>> GetAll();
  Task<int?> Count();
  public Task<T?> GetById(Guid id);
}
