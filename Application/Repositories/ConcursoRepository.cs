using Apllication.Data.Repositories;
using Apllication.Repositories.Interfaces;
using Domain.Entities;
using Domain.Entities.Questions;
using Persistence.Data;

namespace Application.Data.Repositories;

public class ConcursoRepository : GenericRepository<Concurso>, IConcursoRepository
{
  public ConcursoRepository(DataContext context)
    : base(context) { }
}
