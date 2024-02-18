using Apllication.DTOs;
using Apllication.Repositories.Interfaces;
using Application.DTOs;
using Application.DTOs.Concurso;
using Application.DTOs.SubjectDto;
using Domain.Entities;
using Domain.Entities.Inharitance;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Application.Data.Repositories;

public class QuestionRepository : IQuestionRepository
{
  private readonly DataContext _context;

  public QuestionRepository(DataContext context)
  {
    _context = context;
  }

  public async Task<IEnumerable<ViewQuestionDto?>> GetAllComplete()
  {
    if (_context.BaseQuestions != null)
    {
      IEnumerable<ViewQuestionDto?> questions = await BaseQuestionQuery()
        .AsQueryable()
        .ToListAsync();
      return questions;
    }
    throw new Exception("Questions repo is not set");
  }

  public async Task<ViewQuestionDto?> GetCompleteById(int id)
  {
    if (_context.BaseQuestions != null)
    {
      ViewQuestionDto? question = await BaseQuestionQuery().SingleOrDefaultAsync(q => q.Id == id);
      return question;
    }
    throw new Exception("Questions repo is not set");
  }

  private IQueryable<ViewQuestionDto?> BaseQuestionQuery()
  {
    return _context
      .BaseQuestions.Include(q => q.Choices)
      .Include(q => q.Concurso)
      .Include(q => q.Concurso.Institute)
      .Include(q => q.Subject)
      .Include(q => q.Subject.StudyArea)
      .Include(q => q.QuestionLevel)
      .Include(q => q.CreatedBy)
      .Select(q => ParseToViewQuestionDto(q));
  }

  private static ViewQuestionDto ParseToViewQuestionDto(Question baseQuestion)
  {
    return new ViewQuestionDto()
    {
      Id = baseQuestion.Id,
      Answer = baseQuestion.Answer,
      Body = baseQuestion.Body,
      Level = baseQuestion.QuestionLevel.Name,
      Subject = baseQuestion.Subject.Name,
      StudyArea = baseQuestion.Subject.StudyArea.Name,
      Concurso = new ViewConcursoDto()
      {
        Name = baseQuestion.Concurso.Name,
        Year = baseQuestion.Concurso.Year,
        InstituteName = baseQuestion.Concurso.Institute.Name,
      },
      Choices =
        baseQuestion.Choices
        ?? new List<Choice>()
        {
          new Choice { Letter = 'A', Text = "Verdadeiro" },
          new Choice { Letter = 'B', Text = "Falso" }
        },
      CreatedAt = baseQuestion.CreatedAt,
      CreatedBy = new UserDto()
      {
        DisplayName = baseQuestion.CreatedBy?.DisplayName ?? "",
        Username = baseQuestion.CreatedBy?.UserName ?? "",
      },
      EditedBy = new UserDto()
      {
        DisplayName = baseQuestion.EditedBy?.DisplayName ?? "",
        Username = baseQuestion.EditedBy?.UserName ?? "",
      },
      LastUpdatedAt = baseQuestion.LastUpdatedAt,
      Tip = baseQuestion.Tip
    };
  }

  public async Task<int?> GetCount()
  {
    if (_context.BaseQuestions != null)
    {
      int? id = await _context.BaseQuestions.CountAsync();
      return id;
    }
    throw new Exception("Questions repo is not set");
  }

  public async Task<int?> GetLastId()
  {
    if (_context.BaseQuestions != null)
    {
      int? id = await _context.BaseQuestions.MaxAsync(q => q.Id);
      return id;
    }
    throw new Exception("Questions repo is not set");
  }

  public async void Add(Question question, string creatorName)
  {
    var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == creatorName);
    question.CreatedBy = user;
    if (question != null)
    {
      _context.BaseQuestions.Add(question);
    }
  }

  public async void Edit(Question question, string editorName)
  {
    var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == editorName);
    question.EditedBy = user;
    if (question != null)
    {
      _context.Entry(question).State = EntityState.Modified;
    }
  }

  public void Remove(Question entity)
  {
    if (entity != null)
    {
      _context.BaseQuestions.Remove(entity);
    }
  }

  public async Task<Question?> GetById(int id)
  {
    Question? entity = await _context.BaseQuestions.FirstOrDefaultAsync(x => x.Id == id);
    return entity;
  }

  public async Task<bool> SaveChanges()
  {
    return await _context.SaveChangesAsync() > 0;
  }
}
