using Apllication.DTOs;
using Apllication.Repositories;
using Domain.Entities.Inharitance;

namespace Apllication.Services.Interfaces;

public interface IQuestionService
{
  public Task<BaseQuestion> Create(CreateQuestionDTO questionDto);
  public Task<bool> Delete(int id);
  public Task<IEnumerable<BaseQuestion?>> GetAllComplete();

  public Task<BaseQuestion?> GetFullById(int id);

  public Task<int> GetCount();
  public Task<int> GetLastId();
}
