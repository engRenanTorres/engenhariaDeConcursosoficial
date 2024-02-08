using Apllication.DTOs;
using Apllication.Repositories.Interfaces;
using Application.DTOs;
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
      IEnumerable<ViewQuestionDto?> questions = await _context
        .BaseQuestions.Include(x => x.Choices)
        .Include(q => q.CreatedBy)
        .Select(q => ParseToViewQuestionDto(q))
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
      ViewQuestionDto? question = await _context
        .BaseQuestions.Include(q => q.Choices)
        .Include(q => q.CreatedBy)
        .Select(q => ParseToViewQuestionDto(q))
        .SingleOrDefaultAsync(u => u.Id == id);
      return question;
    }
    throw new Exception("Questions repo is not set");
  }

  private static ViewQuestionDto ParseToViewQuestionDto(BaseQuestion baseQuestion)
  {
    return new ViewQuestionDto()
    {
      Id = baseQuestion.Id,
      Answer = baseQuestion.Answer,
      Body = baseQuestion.Body,
      Choices = baseQuestion.Choices,
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

  public async void Add(BaseQuestion question, string creatorName)
  {
    var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == creatorName);
    question.CreatedBy = user;
    if (question != null)
    {
      _context.BaseQuestions.Add(question);
    }
  }

  public async void Edit(BaseQuestion question, string editorName)
  {
    var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == editorName);
    question.EditedBy = user;
    if (question != null)
    {
      _context.Entry(question).State = EntityState.Modified;
    }
  }

  public void Remove(BaseQuestion entity)
  {
    if (entity != null)
    {
      _context.BaseQuestions.Remove(entity);
    }
  }

  public async Task<BaseQuestion?> GetById(int id)
  {
    BaseQuestion? entity = await _context.BaseQuestions.FirstOrDefaultAsync(x => x.Id == id);
    return entity;
  }

  public async Task<bool> SaveChanges()
  {
    return await _context.SaveChangesAsync() > 0;
  }
}
