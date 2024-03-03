using Apllication.Core;
using Apllication.DTOs;
using Apllication.Repositories.Interfaces;
using Application.Core.PagedList;
using Application.DTOs;
using Application.DTOs.Concurso;
using Domain.Entities;
using Domain.Entities.Questions;
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

  public async Task<PagedList<ViewQuestionDto?>> GetAllComplete(PagingParams pagingParams)
  {
    if (_context.Questions != null)
    {
      var questionsQuery = BaseQuestionQuery().AsQueryable();
      var pagedList = await PagedList<ViewQuestionDto>.CreateAsync(
        questionsQuery,
        pagingParams.PageNumber,
        pagingParams.PageSize
      );
      return pagedList;
    }
    throw new Exception("Questions repo is not set");
  }

  public async Task<ViewQuestionDto?> GetCompleteById(int id)
  {
    if (_context.Questions != null)
    {
      ViewQuestionDto? question = await BaseQuestionQuery().SingleOrDefaultAsync(q => q.Id == id);
      return question;
    }
    throw new Exception("Questions repo is not set");
  }

  private IQueryable<ViewQuestionDto?> BaseQuestionQuery()
  {
    return _context
      .Questions.Include(q => ((ChoicesQuestion)q).Choices)
      .Include(q => q.Concurso)
      .Include(q => q.Concurso.Institute)
      .Include(q => q.Subject)
      .Include(q => q.Subject.StudyArea)
      .Include(q => q.QuestionLevel)
      .Include(q => q.InsertedBy)
      .Select(q => ParseToViewQuestionDto(q));
  }

  private static ViewQuestionDto ParseToViewQuestionDto(Question baseQuestion)
  {
    var viewQuestion = new ViewQuestionDto()
    {
      Id = baseQuestion.Id,
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
      InsertedAt = baseQuestion.InsertedAt,
      InsertedBy = new UserDto()
      {
        DisplayName = baseQuestion.InsertedBy?.DisplayName ?? "",
        Id = baseQuestion.InsertedBy?.Id ?? "",
      },
      EditedBy = new UserDto()
      {
        DisplayName = baseQuestion.EditedBy?.DisplayName ?? "",
        Id = baseQuestion.EditedBy?.Id ?? "",
      },
      LastUpdatedAt = baseQuestion.LastUpdatedAt,
      Tip = baseQuestion.Tip
    };
    if (baseQuestion is ChoicesQuestion)
    {
      viewQuestion.Choices = ((ChoicesQuestion)baseQuestion).Choices ?? new List<Choice>();
      viewQuestion.Answer = ((ChoicesQuestion)baseQuestion).Answer;
      if (viewQuestion.Choices.Count == 0)
      {
        viewQuestion.Choices.Add(new Choice { Letter = 'A', Text = "Verdadeiro" });
        viewQuestion.Choices.Add(new Choice { Letter = 'B', Text = "Falso" });
      }
    }
    return viewQuestion;
  }

  public async Task<int?> GetCount()
  {
    if (_context.Questions != null)
    {
      int? id = await _context.Questions.CountAsync();
      return id;
    }
    throw new Exception("Questions repo is not set");
  }

  public async Task<int?> GetLastId()
  {
    if (_context.Questions != null)
    {
      int? id = await _context.Questions.MaxAsync(q => q.Id);
      return id;
    }
    throw new Exception("Questions repo is not set");
  }

  public async void Add(Question question, string creatorName)
  {
    var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == creatorName);
    question.InsertedBy = user;
    if (question != null)
    {
      _context.Questions.Add(question);
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
      _context.Questions.Remove(entity);
    }
  }

  public async Task<Question?> GetById(int id)
  {
    Question? entity = await _context.Questions.FirstOrDefaultAsync(x => x.Id == id);
    return entity;
  }

  public async Task<bool> SaveChanges()
  {
    return await _context.SaveChangesAsync() > 0;
  }
}
