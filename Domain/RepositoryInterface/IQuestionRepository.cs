using Domain.Entities;
using Domain.Entities.Inharitance;

namespace Apllication.Repositories;

public interface IQuestionRepository : IGenericRepository<BaseQuestion>
{
    public Task<IEnumerable<BaseQuestion?>> GetAllComplete();
    public Task<BaseQuestion> GetFullById(int id);
}
