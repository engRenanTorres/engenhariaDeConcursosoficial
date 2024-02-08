using Apllication.Data.Repositories;
using Apllication.Repositories.Interfaces;
using Domain.Entities;
using Domain.Entities.Inharitance;
using Persistence.Data;

namespace Application.Data.Repositories;

public class InstituteRepository : GenericRepository<Institute>, IInstituteRepository
{
  public InstituteRepository(DataContext context)
    : base(context) { }
}
