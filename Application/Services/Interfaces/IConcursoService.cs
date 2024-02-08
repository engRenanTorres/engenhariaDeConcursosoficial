using Apllication.DTOs.Concurso;
using Application.DTOs.Concurso;
using Domain.Entities;
using Domain.Entities.Inharitance;

namespace Apllication.Services.Interfaces;

public interface IConcursoService
{
  public Task<Concurso> Create(CreateConcursoDto dto);
  public Task Delete(Guid id);
  public Task<IEnumerable<Concurso?>> GetAll();

  public Task<Concurso?> GetById(Guid id);

  public Task<int> GetCount();
  public Task<Concurso> Patch(Guid id, UpdateConcursoDto dto);
}
