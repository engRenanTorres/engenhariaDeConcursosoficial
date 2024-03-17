using Apllication.Core;
using Apllication.DTOs.QLevel;
using Apllication.Exceptions;
using Apllication.Repositories.Interfaces;
using Apllication.Services.Interfaces;
using Application.Core.PagedList;
using Application.DTOs.QLevel;
using Application.Errors.Exceptions;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Apllication.Services;

public class QLevelService : Interfaces.IQLevelService
{
  private readonly IQuestionLevelRepository _qlevelRepository;
  private readonly ILogger<Interfaces.IQLevelService> _logger;

  public QLevelService(
    IQuestionLevelRepository subjectRepository,
    ILogger<Interfaces.IQLevelService> logger
  )
  {
    _qlevelRepository = subjectRepository;
    _logger = logger;
  }

  public async Task<IEnumerable<QuestionLevel?>> GetAll()
  {
    var qlevels = await _qlevelRepository.GetAll();
    return qlevels;
  }

  public async Task<PagedList<QuestionLevel?>> GetAllPaged(PagingParams pagingParams)
  {
    var qlevels = await _qlevelRepository.GetAllPaged(pagingParams);
    return qlevels;
  }

  public async Task<QuestionLevel?> GetById(Guid id)
  {
    var qlevel = await _qlevelRepository.GetById(id);
    return qlevel ?? throw new NotFoundException("subject id: " + id + " not found");
  }

  public async Task<int> GetCount()
  {
    int? numberOfqlevels = await _qlevelRepository.Count();
    return numberOfqlevels ?? 0;
  }

  public async Task<QuestionLevel> Create(CreateQLevelDto createDto)
  {
    QuestionLevel qlevel =
      new()
      {
        Name = createDto.Name,
        About = createDto.About,
        CreatedAt = DateTime.Now
      };

    _qlevelRepository.Add(qlevel);
    if (await _qlevelRepository.SaveChanges())
    {
      return qlevel;
    }
    throw new DatabaseException("Error saving qlevel");
  }

  public async Task Delete(Guid id)
  {
    _logger.LogInformation("Delete QLevel has been called.");

    QuestionLevel qlevel =
      await _qlevelRepository.GetById(id)
      ?? throw new NotFoundException("QLevel id: " + id + " not found");

    _qlevelRepository.Remove(qlevel);
    if (await _qlevelRepository.SaveChanges())
      return;
    throw new DatabaseException("Error while deleting qlevel " + id);
  }

  public async Task<QuestionLevel> Patch(Guid id, UpdateQLevelDto updateDTO)
  {
    _logger.LogInformation("Patch QLevelService has been called.");
    QuestionLevel area =
      await _qlevelRepository.GetById(id)
      ?? throw new NotFoundException("QLevel id: " + id + " not found");

    if (updateDTO.Name != null)
      area.Name = updateDTO.Name;
    if (updateDTO.About != null)
      area.About = updateDTO.About;

    if (await _qlevelRepository.SaveChanges())
    {
      _logger.LogInformation("Patch QLevelService has updated question successfully.");
      return area;
    }
    throw new DatabaseException("Error to update QLevel");
  }
}
