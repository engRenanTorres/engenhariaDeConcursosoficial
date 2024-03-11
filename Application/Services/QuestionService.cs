using Apllication.Core;
using Apllication.DTO;
using Apllication.DTOs;
using Apllication.Exceptions;
using Apllication.Interfaces;
using Apllication.Repositories.Interfaces;
using Apllication.Services.Interfaces;
using Application.Core.PagedList;
using Application.DTOs;
using Application.Errors.Exceptions;
using Domain.Entities;
using Domain.Entities.Questions;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Apllication.Services;

public class QuestionService : IQuestionService
{
  private readonly IQuestionRepository _questionRepository;
  private readonly ISubjectService _subjectService;
  private readonly IQLevelService _qlevelService;
  private readonly IConcursoService _concursoService;
  private readonly IUserAccessor _userAccessor;
  private readonly ILogger<IQuestionService> _logger;

  public QuestionService(
    IQuestionRepository questionRepository,
    ISubjectService subjectService,
    IQLevelService qlevelService,
    IConcursoService concursoService,
    IUserAccessor userAccessor,
    ILogger<IQuestionService> logger
  )
  {
    _questionRepository = questionRepository;
    _subjectService = subjectService;
    _qlevelService = qlevelService;
    _concursoService = concursoService;
    _userAccessor = userAccessor;
    _logger = logger;
  }

  public async Task<PagedList<ViewQuestionDto?>> GetAllComplete(QuestionParams pagingParams)
  {
    var questions = await _questionRepository.GetAllComplete(pagingParams);
    return questions;
  }

  public async Task<ViewQuestionDto?> GetFullById(int id)
  {
    var question = await _questionRepository.GetCompleteById(id);
    return question ?? throw new NotFoundException("Question id: " + id + " not found");
  }

  public async Task<int> GetCount()
  {
    int? numberOfQuestion = await _questionRepository.GetCount();
    return numberOfQuestion ?? 0;
  }

  public async Task<int> GetLastId()
  {
    int? numberOfQuestion = await _questionRepository.GetLastId();
    return numberOfQuestion ?? 0;
  }

  public async Task<Question> Create(CreateQuestionDTO questionDto)
  {
    var creatorName =
      _userAccessor.GetUsername() ?? throw new BadRequestException("You must be loged to create.");
    Subject subject =
      await _subjectService.GetById(questionDto.SubjectId)
      ?? throw new NotFoundException("Subject not found.");
    Concurso concurso =
      await _concursoService.GetById(questionDto.ConcursoId)
      ?? throw new NotFoundException("Concurso not found.");
    QuestionLevel qlevel =
      await _qlevelService.GetById(questionDto.LevelId)
      ?? throw new NotFoundException("Level not found.");

    Question question;

    if (questionDto.Answer != null)
    {
      question = new ChoicesQuestion()
      {
        Body = questionDto.Body,
        Answer = questionDto.Answer.ToCharArray()[0],
        Tip = questionDto.Tip ?? "",
        InsertedAt = DateTime.UtcNow,
        Subject = subject,
        Concurso = concurso,
        QuestionLevel = qlevel,
      };
      if (questionDto.Choices != null)
      {
        var choices = new List<Choice>();
        foreach (var choice in questionDto.Choices)
        {
          choices.Add(new Choice() { Letter = choice.Letter.ToCharArray()[0], Text = choice.Text });
        }
        ((ChoicesQuestion)question).Choices = choices;
      }
    }
    else
    {
      question = new DiscursiveQuestion()
      {
        Body = questionDto.Body,
        Tip = questionDto.Tip ?? "",
        InsertedAt = DateTime.UtcNow,
        Subject = subject,
        Concurso = concurso,
        QuestionLevel = qlevel,
      };
    }

    _questionRepository.Add(question, creatorName);
    if (await _questionRepository.SaveChanges())
    {
      return question;
    }
    throw new DatabaseException("Erro ao salvar a questão");
  }

  public async Task Delete(int id)
  {
    _logger.LogInformation("Delete Question has been called.");

    Question question =
      await _questionRepository.GetById(id)
      ?? throw new NotFoundException("Question id: " + id + " not found");

    _questionRepository.Remove(question);
    if (await _questionRepository.SaveChanges())
      return;
    throw new DatabaseException("Error while deleting question " + id);
  }

  public async Task<Question> PatchQuestion(int id, UpdateQuestionDTO updateQuestionDTO)
  {
    _logger.LogInformation("Patch QuestionService has been called.");
    Question question =
      await _questionRepository.GetById(id)
      ?? throw new NotFoundException("Question id: " + id + " not found");

    if (updateQuestionDTO.Body != null)
      question.Body = updateQuestionDTO.Body;
    if (updateQuestionDTO.Tip != null)
      question.Tip = updateQuestionDTO.Tip;
    question.LastUpdatedAt = DateTime.UtcNow;
    if (question is ChoicesQuestion)
    {
      if (updateQuestionDTO.Answer != null)
        ((ChoicesQuestion)question).Answer = (char)updateQuestionDTO.Answer;
      if (updateQuestionDTO.Choices != null)
      {
        var choices = new List<Choice>();
        foreach (var choice in updateQuestionDTO.Choices)
        {
          choices.Add(new Choice() { Letter = choice.Letter.ToCharArray()[0], Text = choice.Text });
        }
        ((ChoicesQuestion)question).Choices = choices;
      }
    }

    var editorName =
      _userAccessor.GetUsername() ?? throw new BadRequestException("You must be loged to edit.");
    _questionRepository.Edit(question, editorName);

    if (await _questionRepository.SaveChanges())
    {
      _logger.LogInformation("Patch QuestionService has updated question successfully.");
      return question;
    }
    throw new DatabaseException("Error to update Question");
  }
}
