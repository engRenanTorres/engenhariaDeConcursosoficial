using Apllication.DTOs;
using Apllication.Exceptions;
using Apllication.Interfaces;
using Apllication.Repositories.Interfaces;
using Apllication.Services.Interfaces;
using Application.DTOs;
using Application.Errors.Exceptions;
using Domain.Entities;
using Domain.Entities.Inharitance;
using Microsoft.Extensions.Logging;

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
    ISubjectService subjectRepository,
    IQLevelService qlevelService,
    IConcursoService concursoService,
    IUserAccessor userAccessor,
    ILogger<IQuestionService> logger
  )
  {
    _questionRepository = questionRepository;
    _subjectService = subjectRepository;
    _qlevelService = qlevelService;
    _concursoService = concursoService;
    _userAccessor = userAccessor;
    _logger = logger;
  }

  public async Task<IEnumerable<ViewQuestionDto?>> GetAllComplete()
  {
    var questions = await _questionRepository.GetAllComplete();
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

  public async Task<BaseQuestion> Create(CreateQuestionDTO questionDto)
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
      await _qlevelService.GetById(questionDto.ConcursoId)
      ?? throw new NotFoundException("Concurso not found.");

    BaseQuestion question;

    if (questionDto.Choices != null)
    {
      var choices = new List<Choice>();
      foreach (var choice in questionDto.Choices)
      {
        choices.Add(new Choice() { Letter = choice.Letter.ToCharArray()[0], Text = choice.Text });
      }
      question = new MultipleChoicesQuestion()
      {
        Body = questionDto.Body,
        Answer = questionDto.Answer.ToCharArray()[0],
        Tip = questionDto.Tip ?? "",
        CreatedAt = DateTime.UtcNow,
        Subject = subject,
        Choices = choices,
        Concurso = concurso,
        QuestionLevel = qlevel,
      };
    }
    else
    {
      question = new BooleanQuestion()
      {
        Body = questionDto.Body,
        Answer = questionDto.Answer.ToCharArray()[0],
        Tip = questionDto.Tip ?? "",
        CreatedAt = DateTime.UtcNow,
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

    BaseQuestion question =
      await _questionRepository.GetById(id)
      ?? throw new NotFoundException("Question id: " + id + " not found");

    _questionRepository.Remove(question);
    if (await _questionRepository.SaveChanges())
      return;
    throw new DatabaseException("Error while deleting question " + id);
  }

  public async Task<BaseQuestion> PatchQuestion(int id, UpdateQuestionDTO updateQuestionDTO)
  {
    _logger.LogInformation("Patch QuestionService has been called.");
    BaseQuestion question =
      await _questionRepository.GetById(id)
      ?? throw new NotFoundException("Question id: " + id + " not found");

    if (updateQuestionDTO.Body != null)
      question.Body = updateQuestionDTO.Body;
    if (updateQuestionDTO.Answer != null)
      question.Answer = (char)updateQuestionDTO.Answer;
    if (updateQuestionDTO.Tip != null)
      question.Tip = updateQuestionDTO.Tip;
    question.LastUpdatedAt = DateTime.UtcNow;
    if (updateQuestionDTO.Choices != null && question is MultipleChoicesQuestion)
    {
      var choices = new List<Choice>();
      foreach (var choice in updateQuestionDTO.Choices)
      {
        choices.Add(new Choice() { Letter = choice.Letter.ToCharArray()[0], Text = choice.Text });
      }
      question.Choices = choices;
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
