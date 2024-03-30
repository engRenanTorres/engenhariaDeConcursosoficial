using Apllication.DTOs.Institute;
using Apllication.Exceptions;
using Apllication.Repositories.Interfaces;
using Apllication.Services.Interfaces;
using Application.DTOs.Institute;
using Application.Errors.Exceptions;
using Domain.Entities;
using Domain.Entities.Questions;
using Microsoft.Extensions.Logging;

namespace Apllication.Services;

public class InstituteService : IInstituteService
{
  private readonly IInstituteRepository _instituteRepository;
  private readonly ILogger<IInstituteService> _logger;

  public InstituteService(
    IInstituteRepository instituteRepository,
    ILogger<IInstituteService> logger
  )
  {
    _instituteRepository = instituteRepository;
    _logger = logger;
  }

  public async Task<IEnumerable<Institute?>> GetAll()
  {
    var institutes = await _instituteRepository.GetAll();
    return institutes;
  }

  public async Task<Institute?> GetById(Guid id)
  {
    var institute = await _instituteRepository.GetById(id);
    return institute ?? throw new NotFoundException("institute id: " + id + " not found");
  }

  public async Task<int> GetCount()
  {
    int? number = await _instituteRepository.Count();
    return number ?? 0;
  }

  public async Task<Institute> Create(CreateInstituteDto createDto)
  {
    Institute institute =
      new()
      {
        Name = createDto.Name,
        About = createDto.About,
        CreatedAt = DateTime.Now
      };

    if (await _instituteRepository.Add(institute))
    {
      return institute;
    }
    throw new DatabaseException("Error saving institute");
  }

  public async Task Delete(Guid id)
  {
    _logger.LogInformation("Delete Institute has been called.");

    Institute institute =
      await _instituteRepository.GetById(id)
      ?? throw new NotFoundException("Institute id: " + id + " not found");

    if (await _instituteRepository.Remove(institute))
      return;
    throw new DatabaseException("Error while deleting institute " + id);
  }

  public async Task<Institute> Patch(Guid id, UpdateInstituteDto updateDTO)
  {
    _logger.LogInformation("Patch InstituteService has been called.");
    Institute institute =
      await _instituteRepository.GetById(id)
      ?? throw new NotFoundException("Institute id: " + id + " not found");

    if (updateDTO.Name != null)
      institute.Name = updateDTO.Name;
    if (updateDTO.About != null)
      institute.About = updateDTO.About;
    if (updateDTO.Contact != null)
      institute.Contact = updateDTO.Contact;

    if (await _instituteRepository.Edit(institute))
    {
      _logger.LogInformation("Patch has updated institute successfully.");
      return institute;
    }
    throw new DatabaseException("Error to update Institute");
  }
}
