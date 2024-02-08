using Apllication.DTOs.Institute;
using Application.DTOs.Institute;
using Domain.Entities;
using Domain.Entities.Inharitance;

namespace Apllication.Services.Interfaces;

public interface IInstituteService
{
  public Task<Institute> Create(CreateInstituteDto dto);
  public Task Delete(Guid id);
  public Task<IEnumerable<Institute?>> GetAll();

  public Task<Institute?> GetById(Guid id);

  public Task<int> GetCount();
  public Task<Institute> Patch(Guid id, UpdateInstituteDto dto);
}
