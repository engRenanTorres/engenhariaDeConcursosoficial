using Domain.Entities.Inharitance;
using Microsoft.EntityFrameworkCore;
using Apllication.Repositories;
using Domain.Entities;

namespace Persistence.Data.Repositories;

public class QuestionRepository : GenericRepository<BaseQuestion>, IQuestionRepository
{
    private readonly DataContext _context;
    public QuestionRepository(DataContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MultipleChoicesQuestion?>> GetAllMultiple()
    {
        if (_context.Questions != null)
        {
            IEnumerable<MultipleChoicesQuestion?> questions = await _context.MultipleChoiceQuestions
              .Include(q => q.ChoiceA)
              .Include(q => q.ChoiceB)
              .Include(q => q.ChoiceC)
              .Include(q => q.ChoiceD)
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