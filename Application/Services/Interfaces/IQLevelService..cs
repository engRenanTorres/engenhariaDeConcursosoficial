using Apllication.Core;
using Apllication.DTOs.Institute;
using Apllication.DTOs.QLevel;
using Application.Core.PagedList;
using Application.DTOs.QLevel;
using Domain.Entities;

namespace Apllication.Services.Interfaces;

public interface IQLevelService
{
  Task<QuestionLevel> Create(CreateQLevelDto dto);
  Task Delete(Guid id);
  Task<IEnumerable<QuestionLevel?>> GetAll();
  Task<PagedList<QuestionLevel?>> GetAllPaged(PagingParams pagingParams);

  Task<QuestionLevel?> GetById(Guid id);

  Task<int> GetCount();
  Task<QuestionLevel> Patch(Guid id, UpdateQLevelDto dto);
}
