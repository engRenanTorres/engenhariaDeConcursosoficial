using System.Runtime.CompilerServices;
using Apllication.Core;
using Apllication.DTO;
using Application.Core.PagedList;
using Application.DTOs;
using Domain.Entities.Questions;

namespace Apllication.Repositories.Interfaces;

public interface IQuestionRepository
{
  Task<PagedList<ViewQuestionDto?>> GetAllComplete(QuestionParams pagingParams);

  Task<int?> GetCount();
  Task<ViewQuestionDto?> GetCompleteById(int id);
  Task<int?> GetLastId();
  Task<bool> Add(Question question, string creatorName);
  Task<bool> Edit(Question question, string editorName);
  Task<Question?> GetById(int id);
  Task<bool> Remove(Question question);
}
