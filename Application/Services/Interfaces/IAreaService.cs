using Apllication.DTOs;
using Apllication.DTOs.StudyArea;
using Apllication.Repositories;
using Application.DTOs;
using Application.DTOs.StudyArea;
using Application.DTOs.SubjectDto;
using Domain.Entities;
using Domain.Entities.Questions;

namespace Apllication.Services.Interfaces;

public interface IAreaService
{
  public Task<StudyArea> Create(CreateAreaDto dto);
  public Task Delete(Guid id);
  public Task<IEnumerable<StudyArea?>> GetAll();

  public Task<StudyArea?> GetById(Guid id);

  public Task<int> GetCount();
  public Task<StudyArea> Patch(Guid id, UpdateAreaDto dto);
}
