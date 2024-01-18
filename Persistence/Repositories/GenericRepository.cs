using Apllication.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Data.Repositories;

public class GenericRepository<T> : IGenericRepository<T>
    where T : class, IEntity
{
    private readonly DataContext _context;

    public GenericRepository(DataContext context)
    {
        if (context.Set<T>() is null)
            throw new Exception($"{nameof(T)} repo is not set");
        _context = context;
    }

    public async Task<bool> SaveChanges()
    {
        return await _context.SaveChangesAsync() > 0;
    }

    public void Add(T entity)
    {
        if (entity != null)
        {
            _context.Set<T>().Add(entity);
        }
    }

    public void Remove(T entity)
    {
        if (entity != null)
        {
            _context.Set<T>().Remove(entity);
        }
    }

    public void Edit(T entity)
    {
        if (entity != null)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }
    }

    public async Task<IEnumerable<T?>> GetAll()
    {
        IEnumerable<T?> entities = await _context.Set<T>().AsQueryable().ToListAsync();

        return entities;
    }

    public virtual async Task<T?> GetById(int id)
    {
        T? entity = await _context.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
        return entity;
    }
}
