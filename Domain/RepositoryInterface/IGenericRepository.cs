namespace Apllication.Repositories;
public interface IGenericRepository<T> where T : class
{
    public Task<bool> SaveChanges();
    public void Add(T entity);
    public void Remove(T entity);
    public Task<IEnumerable<T?>> GetAll();
    public Task<T?> GetById(int id);
}

public interface IEntity 
{
    int Id { get; set; }
}