using System.ComponentModel;
using Apllication.DTOs;
using Apllication.Exceptions;
using Apllication.Repositories;
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

  //private readonly IUserService _userService;
  private readonly ILogger<IQuestionService> _logger;
  private readonly IQuestionService _questionService;
  private readonly MultipleChoicesQuestion _question =
    new()
    {
      Id = 1,
      CreatedAt = new DateTime(2020, 07, 02, 22, 59, 59),
      LastUpdatedAt = new DateTime(2020, 07, 02, 22, 59, 59),
      Body = "Is this a quetion Test?",
      Answer = 'A',
      Tip = "A",
      CreatedById = 1,
      //CreatedBy = null,
    };

  public QuestionServiceTest()
  {
    _questionRepository = A.Fake<IQuestionRepository>();
    //_userService = A.Fake<IUserService>();
    _logger = A.Fake<ILogger<IQuestionService>>();
    _questionService = new QuestionService(_questionRepository, _logger);
  }

  [Fact]
  public async Task GetQuestion_BDContainTheQuestion_ShouldReturnQuestion()
  {
    var id = 1;
    A.CallTo(() => _questionRepository.GetCompleteById(id))
      .Returns(Task.FromResult<BaseQuestion>(_question));

    var result = await _questionService.GetFullById(id);

    result?.Should().BeOfType<MultipleChoicesQuestion>();
    result?.Should().BeSameAs(_question);
  }

  [Fact]
  public async Task GetAllQuestion_BDContainTheQuestion_ShouldReturnQuestions()
  {
    var questions = new List<MultipleChoicesQuestion> { _question };
    A.CallTo(() => _questionRepository.GetAllComplete()).Returns(questions);

    var result = await _questionService.GetAllComplete();

    result?.Should().BeOfType<List<MultipleChoicesQuestion>>();

    result?.Should().BeSameAs(questions);
  }

  [Fact]
  public async Task DeleteQuestion_BDContainTheQuestion_ShouldReturnTrue()
  {
    var questionId = 1;

    A.CallTo(() => _questionRepository.GetById(questionId))
      .Returns(Task.FromResult<BaseQuestion?>(_question));
    A.CallTo(() => _questionRepository.Remove(_question));
    A.CallTo(() => _questionRepository.SaveChanges()).Returns(Task.FromResult<bool>(true));

    var result = await _questionService.Delete(questionId);

    result.Should().Be(true);
  }

  [Fact]
  public async Task DeleteQuestion_NotFoundQuestion_ShouldThrowWarning()
  {
    var questionId = 1;

    A.CallTo(() => _questionRepository.GetById(questionId))
      .Returns(Task.FromResult<BaseQuestion?>(null));

    try
    {
      var result = await _questionService.Delete(questionId);
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
      Tip = "A",
      Choices = new List<ChoiceDTO>()
      {
        new() { Letter = "A", Text = "Alternativa Test" },
        new() { Letter = "B", Text = "Alternativa2 Test" },
      }
    };

    A.CallTo(() => _questionRepository.Add(_question));
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
      Tip = "A",
    };
    /*var userId = "1";
    User _user =
      new()
      {
        Id = 1,
        Name = "Test User",
        Email = "x@x.com",
        Password = new byte[] { Convert.ToByte('p') },
        Role = DotnetAPI.Enums.Roles.User,
        Website = null,
        SocialMedia = null,
      };*/

    A.CallTo(() => _questionRepository.Add(_question));
    //A.CallTo(() => _userService.GetUser(int.Parse(userId))).Returns(Task.FromResult<User?>(_user));
    A.CallTo(() => _questionRepository.SaveChanges()).Returns(Task.FromResult<bool>(true));

    var result = await _questionService.Create(questionDTO);

    result?.Answer.Should().Be(_question.Answer);
    result?.Body.Should().Be(_question.Body);
    result?.Body.Should().Be(_question.Body);
    result.Should().BeOfType<BooleanQuestion>();
  }
}
