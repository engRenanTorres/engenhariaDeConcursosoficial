using Domain.Entities;
using Domain.Entities.Inharitance;

namespace Apllication.Repositories;

public interface IQuestionRepository : IGenericRepository<BaseQuestion>
{
  public Task<IEnumerable<BaseQuestion?>> GetAllComplete();
  public Task<int?> GetCount();
  public Task<BaseQuestion?> GetCompleteById(int id);
  public Task<int?> GetLastId();
}
