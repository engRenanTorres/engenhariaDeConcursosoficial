using Apllication.DTOs;
using Apllication.Exceptions;
using Apllication.Repositories.Interfaces;
using Apllication.Services.Interfaces;
using Application.DTOs.SubjectDto;
using Application.Errors.Exceptions;
using Domain.Entities;
using Domain.Entities.Questions;
using Microsoft.Extensions.Logging;

namespace Apllication.Services;

public class SubjectService : ISubjectService
{
  private readonly ISubjectRepository _subjectRepository;
  private readonly ILogger<ISubjectService> _logger;

  public IAreaService _areaService;

  public SubjectService(
    ISubjectRepository subjectRepository,
    IAreaService areaService,
    ILogger<ISubjectService> logger
  )
  {
    _subjectRepository = subjectRepository;
    _areaService = areaService;
    _logger = logger;
  }

  public async Task<IEnumerable<ViewSubjectDto?>> GetAllComplete()
  {
    var subjects = await _subjectRepository.GetAllComplete();
    return subjects;
  }

  public async Task<ViewSubjectDto?> GetFullById(Guid id)
  {
    var subject = await _subjectRepository.GetCompleteById(id);
    return subject ?? throw new NotFoundException("Subject id: " + id + " not found");
  }

  public async Task<Subject?> GetById(Guid id)
  {
    var subject = await _subjectRepository.GetById(id);
    return subject ?? throw new NotFoundException("Subject id: " + id + " not found");
  }

  public async Task<int> GetCount()
  {
    int? numberOfSubjects = await _subjectRepository.Count();
    return numberOfSubjects ?? 0;
  }

  public async Task<Subject> Create(CreateSubjectDto subjectDto)
  {
    StudyArea studyArea =
      await _areaService.GetById(subjectDto.AreaId)
      ?? throw new NotFoundException("Study Area not found!");
    Subject subject =
      new()
      {
        Name = subjectDto.Name,
        About = subjectDto.About,
        StudyArea = studyArea,
        CreatedAt = DateTime.Now
      };

    if (await _subjectRepository.Add(subject))
    {
      return subject;
    }
    throw new DatabaseException("Error saving");
  }

  public async Task Delete(Guid id)
  {
    _logger.LogInformation("Delete Subject has been called.");

    Subject subject =
      await _subjectRepository.GetById(id)
      ?? throw new NotFoundException("Subject id: " + id + " not found");

    if (await _subjectRepository.Remove(subject))
      return;
    throw new DatabaseException("Error while deleting subject " + id);
  }

  public async Task<Subject> Patch(Guid id, UpdateSubjectDto updateDto)
  {
    _logger.LogInformation("Patch SubjectService has been called.");
    Subject subject =
      await _subjectRepository.GetById(id)
      ?? throw new NotFoundException("Subject id: " + id + " not found");

    if (updateDto.Name != null)
      subject.Name = updateDto.Name;
    if (updateDto.About != null)
      subject.About = updateDto.About;
    if (updateDto.AreaId != null && updateDto.AreaId != Guid.Empty)
    {
      StudyArea studyArea =
        await _areaService.GetById(updateDto.AreaId.Value)
        ?? throw new NotFoundException("Study Area not found!");
      subject.StudyArea = studyArea;
    }

    if (await _subjectRepository.Edit(subject))
    {
      _logger.LogInformation("Patch SubjectService has updated question successfully.");
      return subject;
    }
    throw new DatabaseException("Error to update Subject");
  }
}
