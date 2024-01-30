using Application.DTOs;
using Domain.Entities.Inharitance;

namespace Apllication.Repositories;

public interface IQuestionRepository
{
  public Task<IEnumerable<ViewQuestionDto?>> GetAllComplete();
  public Task<int?> GetCount();
  public Task<ViewQuestionDto?> GetCompleteById(int id);
  public Task<int?> GetLastId();
  void Add(BaseQuestion question, string creatorName);
  Task<bool> SaveChanges();
  void Edit(BaseQuestion question, string editorName);
  Task<BaseQuestion?> GetById(int id);
  void Remove(BaseQuestion question);
}
