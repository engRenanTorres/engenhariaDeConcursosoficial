using Apllication.Core;
using Application.Core.PagedList;
using Application.DTOs;
using Domain.Entities.Questions;

namespace Apllication.Repositories.Interfaces;

public interface IQuestionRepository
{
  public Task<PagedList<ViewQuestionDto?>> GetAllComplete(PagingParams pagingParams);
  public Task<int?> GetCount();
  public Task<ViewQuestionDto?> GetCompleteById(int id);
  public Task<int?> GetLastId();
  void Add(Question question, string creatorName);
  Task<bool> SaveChanges();
  void Edit(Question question, string editorName);
  Task<Question?> GetById(int id);
  void Remove(Question question);
}
