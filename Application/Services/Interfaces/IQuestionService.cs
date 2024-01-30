using Apllication.DTOs;
using Apllication.Repositories;
using Application.DTOs;
using Domain.Entities.Inharitance;

namespace Apllication.Services.Interfaces;

public interface IQuestionService
{
  public Task<BaseQuestion> Create(CreateQuestionDTO questionDto);
  public Task Delete(int id);
  public Task<IEnumerable<ViewQuestionDto?>> GetAllComplete();

  public Task<ViewQuestionDto?> GetFullById(int id);

  public Task<int> GetCount();
  public Task<int> GetLastId();
  public Task<BaseQuestion> PatchQuestion(int id, UpdateQuestionDTO updateQuestionDTO);
}
