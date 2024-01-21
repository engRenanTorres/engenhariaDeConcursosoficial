using Apllication.Repositories;
using Domain.Entities;
using Domain.Entities.Inharitance;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Data.Repositories;

public class QuestionRepository : GenericRepository<BaseQuestion>, IQuestionRepository
{
  private readonly DataContext _context;

  public QuestionRepository(DataContext context)
    : base(context)
  {
    _context = context;
  }

  public async Task<IEnumerable<BaseQuestion?>> GetAllComplete()
  {
    if (_context.BaseQuestions != null)
    {
      IEnumerable<BaseQuestion?> questions = await _context
        .BaseQuestions.Include(x => x.Choices)
        .AsQueryable()
        .ToListAsync();
      return questions;
    }
    throw new Exception("Questions repo is not set");
  }

  public async Task<int> GetCount()
  {
    if (_context.BaseQuestions != null)
    {
      int? id = await _context.BaseQuestions.CountAsync();
      return id ?? throw new KeyNotFoundException();
    }
    throw new Exception("Questions repo is not set");
  }

  public async Task<BaseQuestion> GetCompleteById(int id)
  {
    if (_context.BaseQuestions != null)
    {
      BaseQuestion? question = await _context
        .BaseQuestions.Include(q => q.Choices)
        .SingleOrDefaultAsync(u => u.Id == id);
      return question ?? throw new KeyNotFoundException();
    }
    throw new Exception("Questions repo is not set");
  }

  public async Task<int> GetLastId()
  {
    if (_context.BaseQuestions != null)
    {
      int? id = await _context.BaseQuestions.MaxAsync(q => q.Id);
      return id ?? throw new KeyNotFoundException();
    }
    throw new Exception("Questions repo is not set");
  }
}
