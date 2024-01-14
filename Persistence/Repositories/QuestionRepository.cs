using Domain.Entities.Inharitance;
using Microsoft.EntityFrameworkCore;
using Apllication.Repositories;

namespace Persistence.Data.Repositories;

class QuestionRepository : GenericRepository<BaseQuestion>, IQuestionRepository
{
    private readonly DataContext _context;
    public QuestionRepository(DataContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<BaseQuestion?>> GetAllWithAuthor()
    {
        if (_context.Questions != null)
        {
            IEnumerable<BaseQuestion?> questions = await _context.Questions
              //.Include(q => q.CreatedBy)
              .AsQueryable()
              .ToListAsync();

            return questions;
        }
        throw new Exception("Questions repo is not set");
    }
    public async Task<BaseQuestion?> GetByIdWithAuthor(int id)
    {
        if (_context.Questions != null)
        {
            BaseQuestion? question = await _context.Questions
              //.Include(q => q.CreatedBy)
              .FirstOrDefaultAsync(u => u.Id == id);
            return question;
        }
        throw new Exception("Questions repo is not set");
    }
}