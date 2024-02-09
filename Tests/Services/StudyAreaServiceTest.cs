using Apllication.DTOs.StudyArea;
using Apllication.Exceptions;
using Apllication.Repositories.Interfaces;
using Apllication.Services;
using Apllication.Services.Interfaces;
using Application.DTOs.StudyArea;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Tests;

public class StudyAreaServiceTest
{
  private readonly IStudyAreaRepository _studyAreaRepository;
  private readonly ILogger<IAreaService> _logger;
  private readonly IAreaService _areaService;
  private readonly StudyArea _studyArea =
    new()
    {
      Id = Guid.NewGuid(),
      Name = "EngCivil",
      CreatedAt = new DateTime(2020, 07, 02, 22, 59, 59),
    };

  private ViewAreaDto _viewAreaDto { get; set; }

  public StudyAreaServiceTest()
  {
    _viewAreaDto = new ViewAreaDto
    {
      Id = _studyArea.Id,
      CreatedAt = _studyArea.CreatedAt,
      Name = _studyArea.Name,
    };
    _studyAreaRepository = A.Fake<IStudyAreaRepository>();
    _logger = A.Fake<ILogger<IAreaService>>();
    _areaService = new AreaService(_studyAreaRepository, _logger);
  }

  [Fact]
  public async Task GetArea_BDContainTheArea_ShouldReturnArea()
  {
    var id = _studyArea.Id;
    A.CallTo(() => _studyAreaRepository.GetById(id)).Returns(Task.FromResult(_studyArea));

    var result = await _areaService.GetById(id);

    result?.Should().BeOfType<StudyArea>();
    result?.Should().BeSameAs(_studyArea);
  }

  [Fact]
  public async Task GetAllArea_BDContainTheArea_ShouldReturnAreas()
  {
    var areas = new List<StudyArea> { _studyArea };
    A.CallTo(() => _studyAreaRepository.GetAll()).Returns(areas);

    var result = await _areaService.GetAll();

    result?.Should().BeOfType<List<StudyArea>>();

    result?.Should().BeSameAs(areas);
  }

  [Fact]
  public async Task DeleteArea_BDContainTheArea_ShouldNotThrow()
  {
    var areaId = _studyArea.Id;

    A.CallTo(() => _studyAreaRepository.GetById(areaId))
      .Returns(Task.FromResult<StudyArea?>(_studyArea));
    A.CallTo(() => _studyAreaRepository.Remove(_studyArea));
    A.CallTo(() => _studyAreaRepository.SaveChanges()).Returns(Task.FromResult<bool>(true));

    Func<Task> act = async () => await _areaService.Delete(areaId);

    await act.Should().NotThrowAsync();
  }

  [Fact]
  public async Task DeleteArea_NotFoundArea_ShouldThrowWarning()
  {
    var areaId = _studyArea.Id;

    A.CallTo(() => _studyAreaRepository.GetById(areaId)).Returns(Task.FromResult<StudyArea?>(null));

    try
    {
      await _areaService.Delete(areaId);
    }
    catch (Exception ex)
    {
      ex.Should().BeOfType<NotFoundException>();
      ex.Message.Should().Be($"Area id: {_studyArea.Id} not found");
    }
  }

  [Fact]
  public async Task PatchArea_updateSucessfully_ShouldReturnArea()
  {
    var updateAreaDTO = new UpdateAreaDto();
    var areaId = _studyArea.Id;

    A.CallTo(() => _studyAreaRepository.GetById(areaId))
      .Returns(Task.FromResult<StudyArea?>(_studyArea));
    A.CallTo(() => _studyAreaRepository.SaveChanges()).Returns(Task.FromResult<bool>(true));

    var result = await _areaService.Patch(areaId, updateAreaDTO);

    result.Should().Be(_studyArea);
  }

  [Fact]
  public async Task PatchArea_NotFoundArea_ShouldThrowNotFoundException()
  {
    var updateAreaDTO = new UpdateAreaDto();
    var quesitonId = _studyArea.Id;

    A.CallTo(() => _studyAreaRepository.GetById(quesitonId))
      .Returns(Task.FromResult<StudyArea?>(null));
    try
    {
      var result = await _areaService.Patch(quesitonId, updateAreaDTO);
    }
    catch (Exception ex)
    {
      ex.Should().BeOfType<NotFoundException>();
    }
  }

  [Fact]
  public async Task CreateArea_ShouldReturnMultipleChoiceArea_WhenCreateAreaWithChoices()
  {
    var areaDTO = new CreateAreaDto() { Name = "EngCivil", };

    A.CallTo(() => _studyAreaRepository.Add(_studyArea));
    A.CallTo(() => _studyAreaRepository.SaveChanges()).Returns(Task.FromResult<bool>(true));

    var result = await _areaService.Create(areaDTO);

    result?.Name.Should().Be(_studyArea.Name);
    result.Should().BeOfType<StudyArea>();
  }

  [Fact]
  public async Task CreateArea_ShouldReturnBooleanArea_WhenCreateAreaWithoutChoices()
  {
    var areaDTO = new CreateAreaDto() { Name = "EngCivil", };

    A.CallTo(() => _studyAreaRepository.Add(_studyArea));
    //A.CallTo(() => _userService.GetUser(int.Parse(userId))).Returns(Task.FromResult<User?>(_user));
    A.CallTo(() => _studyAreaRepository.SaveChanges()).Returns(Task.FromResult<bool>(true));

    var result = await _areaService.Create(areaDTO);

    result?.Name.Should().Be(_studyArea.Name);
    result.Should().BeOfType<StudyArea>();
  }
}
