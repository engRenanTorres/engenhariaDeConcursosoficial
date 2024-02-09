using Apllication.DTOs;
using Apllication.Exceptions;
using Apllication.Interfaces;
using Apllication.Repositories.Interfaces;
using Apllication.Services;
using Apllication.Services.Interfaces;
using Application.DTOs;
using Domain.Entities;
using Domain.Entities.Inharitance;
using Microsoft.Extensions.Logging;

namespace Tests;

public class QuestionServiceTest
{
  private readonly IQuestionRepository _questionRepository;

  private readonly IUserAccessor _userAccessor;
  private readonly ILogger<IQuestionService> _logger;
  private readonly IQuestionService _questionService;
  private readonly ISubjectService _subjectService;
  private readonly IConcursoService _concursoService;
  private readonly IQLevelService _qLevelService;
  private readonly MultipleChoicesQuestion _question;
  private readonly DateTime _createdAt = new(2020, 07, 02, 22, 59, 59);
  private readonly ViewQuestionDto _viewQuestionDto;
  private readonly AppUser _appUser =
    new()
    {
      DisplayName = "TestUser",
      Email = "test@tes.com",
      UserName = "tester"
    };
  private readonly QuestionLevel _questionLevel =
    new()
    {
      Id = Guid.NewGuid(),
      Name = "Superior",
      CreatedAt = new DateTime(2020, 07, 02, 22, 59, 59),
    };
  private readonly Concurso _concurso =
    new()
    {
      Id = Guid.NewGuid(),
      Name = "Petrobras",
      CreatedAt = new DateTime(2020, 07, 02, 22, 59, 59),
      Institute = new() { Id = Guid.NewGuid(), Name = "Cebraspe" }
    };
  private readonly Subject _subject =
    new()
    {
      Id = Guid.NewGuid(),
      Name = "Estruturas",
      StudyArea = new() { Id = Guid.NewGuid(), Name = "EngCivil", }
    };

  public QuestionServiceTest()
  {
    _question = new()
    {
      Id = 1,
      CreatedBy = _appUser,
      QuestionLevel = _questionLevel,
      Concurso = _concurso,
      Subject = _subject,
      CreatedAt = _createdAt,
      LastUpdatedAt = new DateTime(2020, 07, 02, 22, 59, 59),
      Body = "Is this a quetion Test?",
      Answer = 'A',
      Tip = "A",
    };
    _viewQuestionDto = new ViewQuestionDto
    {
      Id = _question.Id,
      CreatedAt = _question.CreatedAt,
      LastUpdatedAt = _question.LastUpdatedAt,
      Body = _question.Body,
      Answer = _question.Answer,
      Tip = _question.Tip,
      CreatedBy = new UserDto
      {
        DisplayName = _question.CreatedBy.DisplayName,
        Username = _question.CreatedBy.UserName,
      }
    };
    _questionRepository = A.Fake<IQuestionRepository>();
    _subjectService = A.Fake<ISubjectService>();
    _qLevelService = A.Fake<IQLevelService>();
    _concursoService = A.Fake<IConcursoService>();
    _userAccessor = A.Fake<IUserAccessor>();
    _logger = A.Fake<ILogger<IQuestionService>>();
    _questionService = new QuestionService(
      _questionRepository,
      _subjectService,
      _qLevelService,
      _concursoService,
      _userAccessor,
      _logger
    );
  }

  [Fact]
  public async Task GetQuestion_BDContainTheQuestion_ShouldReturnQuestion()
  {
    var id = 1;
    A.CallTo(() => _questionRepository.GetCompleteById(id))
      .Returns(Task.FromResult(_viewQuestionDto));

    var result = await _questionService.GetFullById(id);

    result?.Should().BeOfType<ViewQuestionDto>();
    result?.Should().BeSameAs(_viewQuestionDto);
  }

  [Fact]
  public async Task GetAllQuestion_BDContainTheQuestion_ShouldReturnQuestions()
  {
    var questions = new List<ViewQuestionDto> { _viewQuestionDto };
    A.CallTo(() => _questionRepository.GetAllComplete()).Returns(questions);

    var result = await _questionService.GetAllComplete();

    result?.Should().BeOfType<List<ViewQuestionDto>>();

    result?.Should().BeSameAs(questions);
  }

  [Fact]
  public async Task DeleteQuestion_BDContainTheQuestion_ShouldNotThrow()
  {
    var questionId = 1;

    A.CallTo(() => _questionRepository.GetById(questionId))
      .Returns(Task.FromResult<BaseQuestion?>(_question));
    A.CallTo(() => _questionRepository.Remove(_question));
    A.CallTo(() => _questionRepository.SaveChanges()).Returns(Task.FromResult<bool>(true));

    Func<Task> act = async () => await _questionService.Delete(questionId);

    await act.Should().NotThrowAsync();
  }

  [Fact]
  public async Task DeleteQuestion_NotFoundQuestion_ShouldThrowWarning()
  {
    var questionId = 1;

    A.CallTo(() => _questionRepository.GetById(questionId))
      .Returns(Task.FromResult<BaseQuestion?>(null));

    try
    {
      await _questionService.Delete(questionId);
    }
    catch (Exception ex)
    {
      ex.Should().BeOfType<NotFoundException>();
      ex.Message.Should().Be("Question id: 1 not found");
    }
  }

  [Fact]
  public async Task PatchQuestion_updateSucessfully_ShouldReturnQuestion()
  {
    var updateQuestionDTO = new UpdateQuestionDTO();
    var questionId = 1;

    A.CallTo(() => _questionRepository.GetById(questionId))
      .Returns(Task.FromResult<BaseQuestion?>(_question));
    A.CallTo(() => _questionRepository.SaveChanges()).Returns(Task.FromResult<bool>(true));

    var result = await _questionService.PatchQuestion(questionId, updateQuestionDTO);

    result.Should().Be(_question);
  }

  [Fact]
  public async Task PatchQuestion_NotFoundQuestion_ShouldThrowNotFoundException()
  {
    var updateQuestionDTO = new UpdateQuestionDTO();
    var quesitonId = 1;

    A.CallTo(() => _questionRepository.GetById(quesitonId))
      .Returns(Task.FromResult<BaseQuestion?>(null));
    try
    {
      var result = await _questionService.PatchQuestion(quesitonId, updateQuestionDTO);
    }
    catch (Exception ex)
    {
      ex.Should().BeOfType<NotFoundException>();
    }
  }

  [Fact]
  public async Task CreateQuestion_ShouldReturnMultipleChoiceQuestion_WhenCreateQuestionWithChoices()
  {
    var questionDTO = new CreateQuestionDTO()
    {
      Body = "Is this a quetion Test?",
      Answer = "A",
      SubjectId = _subject.Id,
      ConcursoId = _concurso.Id,
      Tip = "A",
      Choices = new List<ChoiceDto>()
      {
        new() { Letter = "A", Text = "Alternativa Test" },
        new() { Letter = "B", Text = "Alternativa2 Test" },
      }
    };

    A.CallTo(() => _questionRepository.Add(_question, _appUser.UserName));
    A.CallTo(() => _questionRepository.SaveChanges()).Returns(Task.FromResult<bool>(true));

    var result = await _questionService.Create(questionDTO);

    result?.Answer.Should().Be(_question.Answer);
    result?.Body.Should().Be(_question.Body);
    result?.Body.Should().Be(_question.Body);
    result.Should().BeOfType<MultipleChoicesQuestion>();
  }

  [Fact]
  public async Task CreateQuestion_ShouldReturnBooleanQuestion_WhenCreateQuestionWithoutChoices()
  {
    var questionDTO = new CreateQuestionDTO()
    {
      Body = "Is this a quetion Test?",
      Answer = "A",
      SubjectId = _subject.Id,
      ConcursoId = _concurso.Id,
      Tip = "A",
    };

    A.CallTo(() => _questionRepository.Add(_question, _appUser.UserName));
    //A.CallTo(() => _userService.GetUser(int.Parse(userId))).Returns(Task.FromResult<User?>(_user));
    A.CallTo(() => _questionRepository.SaveChanges()).Returns(Task.FromResult<bool>(true));

    var result = await _questionService.Create(questionDTO);

    result?.Answer.Should().Be(_question.Answer);
    result?.Body.Should().Be(_question.Body);
    result?.Body.Should().Be(_question.Body);
    result.Should().BeOfType<BooleanQuestion>();
  }
}
