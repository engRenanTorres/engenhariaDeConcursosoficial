using Apllication.Data.Repositories;
using Apllication.Repositories.Interfaces;
using Domain.Entities;
using Domain.Entities.Inharitance;
using Persistence.Data;

namespace Application.Data.Repositories;

public class QuestionLevelRepository : GenericRepository<QuestionLevel>, IQuestionLevelRepository
{
  public QuestionLevelRepository(DataContext context)
    : base(context) { }
}
