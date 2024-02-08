using Apllication.DTOs.Institute;
using Apllication.DTOs.QLevel;
using Application.DTOs.QLevel;
using Domain.Entities;

namespace Apllication.Services.Interfaces;

public interface IQLevelService
{
  public Task<QuestionLevel> Create(CreateQLevelDto dto);
  public Task Delete(Guid id);
  public Task<IEnumerable<QuestionLevel?>> GetAll();

  public Task<QuestionLevel?> GetById(Guid id);

  public Task<int> GetCount();
  public Task<QuestionLevel> Patch(Guid id, UpdateQLevelDto dto);
}
