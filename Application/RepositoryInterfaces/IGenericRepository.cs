using Apllication.Core;
using Application.Core.PagedList;

namespace Apllication.Repositories.Interfaces;

public interface IGenericRepository<T>
  where T : class
{
  Task<bool> SaveChanges();
  void Add(T entity);
  void Remove(T entity);
  Task<IEnumerable<T?>> GetAll();
  Task<int?> Count();
  Task<T?> GetById(Guid id);

  Task<PagedList<T?>> GetAllPaged(PagingParams pagingParams);
}
