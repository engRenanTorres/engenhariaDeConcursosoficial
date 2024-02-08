using Apllication.Data.Repositories;
using Apllication.Repositories.Interfaces;
using Domain.Entities;
using Domain.Entities.Inharitance;
using Persistence.Data;

namespace Application.Data.Repositories;

public class StudyAreaRepository : GenericRepository<StudyArea>, IStudyAreaRepository
{
  public StudyAreaRepository(DataContext context)
    : base(context) { }
}
