using Domain.Entities;
using Domain.Entities.Inharitance;

namespace Apllication.Repositories;
public interface IQuestionRepository : IGenericRepository<BaseQuestion>
{
  public Task<IEnumerable<MultipleChoicesQuestion?>> GetAllMultiple();

}