using Apllication.DTOs.QLevel;
using Apllication.Exceptions;
using Apllication.Repositories.Interfaces;
using Apllication.Services;
using Apllication.Services.Interfaces;
using Application.DTOs.QLevel;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Tests;

public class QuestionLevelServiceTest
{
  private readonly IQuestionLevelRepository _questionLevelRepository;
  private readonly ILogger<IQLevelService> _logger;
  private readonly IQLevelService _questionLevelService;
  private readonly QuestionLevel _questionLevel =
    new()
    {
      Id = Guid.NewGuid(),
      Name = "Superior",
      CreatedAt = new DateTime(2020, 07, 02, 22, 59, 59),
    };

  private ViewQLevelDto _viewQLevelnDto { get; set; }

  public QuestionLevelServiceTest()
  {
    _viewQLevelnDto = new ViewQLevelDto
    {
      Id = _questionLevel.Id,
      CreatedAt = _questionLevel.CreatedAt,
      Name = _questionLevel.Name,
    };
    _questionLevelRepository = A.Fake<IQuestionLevelRepository>();
    _logger = A.Fake<ILogger<IQLevelService>>();
    _questionLevelService = new QLevelService(_questionLevelRepository, _logger);
  }

  [Fact]
  public async Task Get_BDContainTheQuestionLevel_ShouldReturnQuestionLevel()
  {
    var id = _questionLevel.Id;
    A.CallTo(() => _questionLevelRepository.GetById(id)).Returns(Task.FromResult(_questionLevel));

    var result = await _questionLevelService.GetById(id);

    result?.Should().BeOfType<QuestionLevel>();
    result?.Should().BeSameAs(_questionLevel);
  }

  [Fact]
  public async Task GetAll_BDContainTheQuestionLevel_ShouldReturnQuestionLevels()
  {
    var questions = new List<QuestionLevel> { _questionLevel };
    A.CallTo(() => _questionLevelRepository.GetAll()).Returns(questions);

    var result = await _questionLevelService.GetAll();

    result?.Should().BeOfType<List<QuestionLevel>>();

    result?.Should().BeSameAs(questions);
  }

  [Fact]
  public async Task Delete_BDContainTheQuestionLevel_ShouldNotThrow()
  {
    var questionId = _questionLevel.Id;

    A.CallTo(() => _questionLevelRepository.GetById(questionId))
      .Returns(Task.FromResult<QuestionLevel?>(_questionLevel));
    A.CallTo(() => _questionLevelRepository.Remove(_questionLevel));
    A.CallTo(() => _questionLevelRepository.SaveChanges()).Returns(Task.FromResult<bool>(true));

    Func<Task> act = async () => await _questionLevelService.Delete(questionId);

    await act.Should().NotThrowAsync();
  }

  [Fact]
  public async Task Delete_NotFoundQuestionLevel_ShouldThrowWarning()
  {
    var questionId = _questionLevel.Id;

    A.CallTo(() => _questionLevelRepository.GetById(questionId))
      .Returns(Task.FromResult<QuestionLevel?>(null));

    try
    {
      await _questionLevelService.Delete(questionId);
    }
    catch (Exception ex)
    {
      ex.Should().BeOfType<NotFoundException>();
      ex.Message.Should().Be($"QLevel id: {_questionLevel.Id} not found");
    }
  }

  [Fact]
  public async Task Patch_updateSucessfully_ShouldReturnQuestion()
  {
    var updateQuestionDTO = new UpdateQLevelDto();
    var questionId = _questionLevel.Id;

    A.CallTo(() => _questionLevelRepository.GetById(questionId))
      .Returns(Task.FromResult<QuestionLevel?>(_questionLevel));
    A.CallTo(() => _questionLevelRepository.SaveChanges()).Returns(Task.FromResult<bool>(true));

    var result = await _questionLevelService.Patch(questionId, updateQuestionDTO);

    result.Should().Be(_questionLevel);
  }

  [Fact]
  public async Task Patch_NotFoundQuestionLevel_ShouldThrowNotFoundException()
  {
    var updateQuestionDTO = new UpdateQLevelDto();
    var quesitonId = _questionLevel.Id;

    A.CallTo(() => _questionLevelRepository.GetById(quesitonId))
      .Returns(Task.FromResult<QuestionLevel?>(null));
    try
    {
      var result = await _questionLevelService.Patch(quesitonId, updateQuestionDTO);
    }
    catch (Exception ex)
    {
      ex.Should().BeOfType<NotFoundException>();
    }
  }

  [Fact]
  public async Task Create_ShouldReturnMultipleChoiceQuestion_WhenCreateQuestionWithChoices()
  {
    var questionDTO = new CreateQLevelDto() { Name = "Superior", };

    A.CallTo(() => _questionLevelRepository.Add(_questionLevel));
    A.CallTo(() => _questionLevelRepository.SaveChanges()).Returns(Task.FromResult<bool>(true));

    var result = await _questionLevelService.Create(questionDTO);

    result?.Name.Should().Be(_questionLevel.Name);
    result.Should().BeOfType<QuestionLevel>();
  }

  [Fact]
  public async Task Create_ShouldReturnBooleanQuestionLevel_WhenCreateQuestionLevelWithoutChoices()
  {
    var questionDTO = new CreateQLevelDto() { Name = "Superior", };

    A.CallTo(() => _questionLevelRepository.Add(_questionLevel));
    A.CallTo(() => _questionLevelRepository.SaveChanges()).Returns(Task.FromResult<bool>(true));

    var result = await _questionLevelService.Create(questionDTO);

    result?.Name.Should().Be(_questionLevel.Name);
    result.Should().BeOfType<QuestionLevel>();
  }
}
