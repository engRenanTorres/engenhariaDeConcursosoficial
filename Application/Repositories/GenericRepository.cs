using System;
using Apllication.Core;
using Apllication.Repositories.Interfaces;
using Application.Core.PagedList;
using Domain.Entities.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Apllication.Data.Repositories;

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

  private async Task<bool> SaveChanges()
  {
    return await _context.SaveChangesAsync() > 0;
  }

  public async Task<bool> Add(T entity)
  {
    if (entity != null)
    {
      entity.CreatedAt = DateTime.UtcNow;
      _context.Set<T>().Add(entity);
      return await this.SaveChanges();
    }
    return false;
  }

  public async Task<bool> Remove(T entity)
  {
    if (entity != null)
    {
      _context.Set<T>().Remove(entity);
      return await this.SaveChanges();
    }
    return false;
  }

  public async Task<bool> Edit(T entity)
  {
    if (entity != null)
    {
      _context.Entry(entity).State = EntityState.Modified;
      return await this.SaveChanges();
    }
    return false;
  }

  public async Task<IEnumerable<T?>> GetAll()
  {
    IEnumerable<T?> entities = await _context.Set<T>().AsQueryable().ToListAsync();

    return entities;
  }

  public async Task<PagedList<T?>> GetAllPaged(PagingParams pagingParams)
  {
    IQueryable<T> entitiesQuery = _context.Set<T>().AsQueryable();
    var pagedList = await PagedList<T>.CreateAsync(
      entitiesQuery,
      pagingParams.PageNumber,
      pagingParams.PageSize
    );

    return pagedList;
  }

  public virtual async Task<T?> GetById(Guid id)
  {
    T? entity = await _context.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
    return entity;
  }

  public async Task<int?> Count()
  {
    if (_context.Set<T>() != null)
    {
      int? id = await _context.Set<T>().CountAsync();
      return id;
    }
    throw new Exception($"{nameof(T)} repo is not set");
  }
}
