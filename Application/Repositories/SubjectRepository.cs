using Apllication.Data.Repositories;
using Apllication.Repositories.Interfaces;
using Application.DTOs.SubjectDto;
using Domain.Entities;
using Domain.Entities.Inharitance;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Application.Data.Repositories;

public class SubjectRepository : GenericRepository<Subject>, ISubjectRepository
{
  public DataContext _context { get; set; }

  public SubjectRepository(DataContext context)
    : base(context)
  {
    _context = context;
  }

  public async Task<IEnumerable<ViewSubjectDto?>> GetAllComplete()
  {
    if (_context.Subjects != null)
    {
      IEnumerable<ViewSubjectDto?> subjects = await _context
        .Subjects.Include(x => x.StudyArea)
        .Select(s => ParseToViewQuestionDto(s))
        .AsQueryable()
        .ToListAsync();
      return subjects;
    }
    throw new Exception("Subject repo is not set");
  }

  public async Task<ViewSubjectDto?> GetCompleteById(Guid id)
  {
    if (_context.Subjects != null)
    {
      ViewSubjectDto? subject = await _context
        .Subjects.Include(x => x.StudyArea)
        .Select(s => ParseToViewQuestionDto(s))
        .Where(s => s.Id == id)
        .AsQueryable()
        .FirstOrDefaultAsync();
      return subject;
    }
    throw new Exception("Subject repo is not set");
  }

  private static DTOs.SubjectDto.ViewSubjectDto ParseToViewQuestionDto(Subject subject)
  {
    return new ViewSubjectDto()
    {
      Id = subject.Id,
      Name = subject.Name,
      About = subject.About,
      Area = subject.StudyArea,
    };
  }
}
