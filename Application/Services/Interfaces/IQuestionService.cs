using Apllication.Core;
using Apllication.DTO;
using Apllication.DTOs;
using Apllication.Repositories;
using Application.Core.PagedList;
using Application.DTOs;
using Domain.Entities.Questions;

namespace Apllication.Services.Interfaces;

public interface IQuestionService
{
  public Task<Question> Create(CreateQuestionDTO questionDto);
  public Task Delete(int id);
  public Task<PagedList<ViewQuestionDto?>> GetAllComplete(QuestionParams pagingParams);

  public Task<ViewQuestionDto?> GetFullById(int id);

  public Task<int> GetCount();
  public Task<int> GetLastId();
  public Task<Question> Patch(int id, UpdateQuestionDTO updateQuestionDTO);
}
