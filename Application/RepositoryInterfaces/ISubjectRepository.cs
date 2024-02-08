using Application.DTOs.SubjectDto;
using Domain.Entities;

namespace Apllication.Repositories.Interfaces;

public interface ISubjectRepository : IGenericRepository<Subject>
{
  Task<IEnumerable<ViewSubjectDto?>> GetAllComplete();
  Task<ViewSubjectDto?> GetCompleteById(Guid id);
}
