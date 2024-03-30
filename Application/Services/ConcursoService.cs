using Apllication.DTOs.Concurso;
using Apllication.Exceptions;
using Apllication.Repositories.Interfaces;
using Apllication.Services.Interfaces;
using Application.DTOs.Concurso;
using Application.Errors.Exceptions;
using Domain.Entities;
using Domain.Entities.Questions;
using Microsoft.Extensions.Logging;

namespace Apllication.Services;

public class ConcursoService : IConcursoService
{
  private readonly IConcursoRepository _concursoRepository;
  private readonly IInstituteService _instituteService;
  private readonly ILogger<IConcursoService> _logger;

  public ConcursoService(
    IConcursoRepository subjectRepository,
    IInstituteService instituteService,
    ILogger<IConcursoService> logger
  )
  {
    _concursoRepository = subjectRepository;
    _instituteService = instituteService;
    _logger = logger;
  }

  public async Task<IEnumerable<Concurso?>> GetAll()
  {
    var concursos = await _concursoRepository.GetAll();
    return concursos;
  }

  public async Task<Concurso?> GetById(Guid id)
  {
    var concurso = await _concursoRepository.GetById(id);
    return concurso ?? throw new NotFoundException("subject id: " + id + " not found");
  }

  public async Task<int> GetCount()
  {
    int? numberOfconcursos = await _concursoRepository.Count();
    return numberOfconcursos ?? 0;
  }

  public async Task<Concurso> Create(CreateConcursoDto createDto)
  {
    Institute institute =
      await _instituteService.GetById(createDto.InstituteId)
      ?? throw new NotFoundException("Institute not found.");
    Concurso concurso =
      new()
      {
        Name = createDto.Name,
        About = createDto.About,
        Year = createDto.Year,
        CreatedAt = DateTime.Now,
        Institute = institute
      };

    if (await _concursoRepository.Add(concurso))
    {
      return concurso;
    }
    throw new DatabaseException("Error saving concurso");
  }

  public async Task Delete(Guid id)
  {
    _logger.LogInformation("Delete concurso has been called.");

    Concurso concurso =
      await _concursoRepository.GetById(id)
      ?? throw new NotFoundException("Concurso id: " + id + " not found");

    if (await _concursoRepository.Remove(concurso))
      return;
    throw new DatabaseException("Error while deleting concurso " + id);
  }

  public async Task<Concurso> Patch(Guid id, UpdateConcursoDto updateDTO)
  {
    _logger.LogInformation("Patch ConcursoService has been called.");
    Concurso concurso =
      await _concursoRepository.GetById(id)
      ?? throw new NotFoundException("Concurso id: " + id + " not found");

    if (updateDTO.Name != null)
      concurso.Name = updateDTO.Name;
    if (updateDTO.About != null)
      concurso.About = updateDTO.About;
    if (updateDTO.Year.HasValue)
      concurso.Year = updateDTO.Year.GetValueOrDefault();

    if (await _concursoRepository.Edit(concurso))
    {
      _logger.LogInformation("Patch ConcursoService has updated question successfully.");
      return concurso;
    }
    throw new DatabaseException("Error to update Concurso");
  }
}
