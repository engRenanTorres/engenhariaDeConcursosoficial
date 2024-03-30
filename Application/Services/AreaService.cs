using Apllication.DTOs.StudyArea;
using Apllication.Exceptions;
using Apllication.Repositories.Interfaces;
using Apllication.Services.Interfaces;
using Application.DTOs.StudyArea;
using Application.Errors.Exceptions;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Apllication.Services;

public class AreaService : IAreaService
{
  private readonly IStudyAreaRepository _areaRepository;
  private readonly ILogger<IAreaService> _logger;

  public AreaService(IStudyAreaRepository subjectRepository, ILogger<IAreaService> logger)
  {
    _areaRepository = subjectRepository;
    _logger = logger;
  }

  public async Task<IEnumerable<StudyArea?>> GetAll()
  {
    var areas = await _areaRepository.GetAll();
    return areas;
  }

  public async Task<StudyArea?> GetById(Guid id)
  {
    var area = await _areaRepository.GetById(id);
    return area ?? throw new NotFoundException("subject id: " + id + " not found");
  }

  public async Task<int> GetCount()
  {
    int? numberOfareas = await _areaRepository.Count();
    return numberOfareas ?? 0;
  }

  public async Task<StudyArea> Create(CreateAreaDto createDto)
  {
    StudyArea area =
      new()
      {
        Name = createDto.Name,
        About = createDto.About,
        CreatedAt = DateTime.Now
      };

    if (await _areaRepository.Add(area))
    {
      return area;
    }
    throw new DatabaseException("Error saving area");
  }

  public async Task Delete(Guid id)
  {
    _logger.LogInformation("Delete Area has been called.");

    StudyArea area =
      await _areaRepository.GetById(id)
      ?? throw new NotFoundException("Area id: " + id + " not found");

    if (await _areaRepository.Remove(area))
      return;
    throw new DatabaseException("Error while deleting area " + id);
  }

  public async Task<StudyArea> Patch(Guid id, UpdateAreaDto updateDTO)
  {
    _logger.LogInformation("Patch AreaService has been called.");
    StudyArea area =
      await _areaRepository.GetById(id)
      ?? throw new NotFoundException("Area id: " + id + " not found");

    if (updateDTO.Name != null)
      area.Name = updateDTO.Name;
    if (updateDTO.About != null)
      area.About = updateDTO.About;

    if (await _areaRepository.Edit(area))
    {
      _logger.LogInformation("Patch AreaService has updated question successfully.");
      return area;
    }
    throw new DatabaseException("Error to update Area");
  }
}
