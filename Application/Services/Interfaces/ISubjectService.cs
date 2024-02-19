using Apllication.DTOs;
using Apllication.Repositories;
using Apllication.Repositories.Interfaces;
using Application.DTOs;
using Application.DTOs.SubjectDto;
using Domain.Entities;
using Domain.Entities.Questions;

namespace Apllication.Services.Interfaces;

public interface ISubjectService
{
  public Task<Subject> Create(CreateSubjectDto dto);
  public Task Delete(Guid id);
  public Task<IEnumerable<ViewSubjectDto?>> GetAllComplete();

  public Task<ViewSubjectDto?> GetFullById(Guid id);
  public Task<Subject?> GetById(Guid id);

  public Task<int> GetCount();
  public Task<Subject> Patch(Guid id, UpdateSubjectDto dto);
}
