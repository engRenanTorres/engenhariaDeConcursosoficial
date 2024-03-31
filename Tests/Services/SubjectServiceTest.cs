using Apllication.DTOs;
using Apllication.Exceptions;
using Apllication.Repositories.Interfaces;
using Apllication.Services;
using Apllication.Services.Interfaces;
using Application.DTOs.SubjectDto;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Tests;

public class SubjectServiceTest
{
  private readonly ISubjectRepository _subjectRepository;
  private readonly ILogger<ISubjectService> _logger;
  private readonly ISubjectService _subjectService;
  private readonly IGenericRepository<Subject> _genericRepository;
  private readonly IAreaService _areaService;
  private readonly StudyArea _studyArea =
    new()
    {
      Id = Guid.NewGuid(),
      Name = "EngCivil",
      CreatedAt = new DateTime(2020, 07, 02, 22, 59, 59),
    };

  private readonly Subject _subject;
  private readonly ViewSubjectDto _subjectDto;

  public SubjectServiceTest()
  {
    _subject = new Subject
    {
      Id = Guid.NewGuid(),
      CreatedAt = new DateTime(2020, 07, 02, 22, 59, 59),
      Name = "Estruturas",
      StudyArea = _studyArea,
    };
    _subjectDto = new() { Name = _subject.Name, };
    _genericRepository = A.Fake<IGenericRepository<Subject>>();
    _subjectRepository = A.Fake<ISubjectRepository>();
    _areaService = A.Fake<IAreaService>();
    _logger = A.Fake<ILogger<ISubjectService>>();
    _subjectService = new SubjectService(_subjectRepository, _areaService, _logger);
  }

  [Fact]
  public async Task GetSubject_BDContainTheSubject_ShouldReturnSubject()
  {
    var id = _subject.Id;
    A.CallTo(() => _subjectRepository.GetById(id)).Returns(Task.FromResult(_subject));

    var result = await _subjectService.GetById(id);

    result?.Should().BeOfType<Subject>();
    result?.Should().BeSameAs(_subject);
  }

  [Fact]
  public async Task GetAllSubject_BDContainTheSubject_ShouldReturnSubjects()
  {
    var subjects = new List<ViewSubjectDto> { _subjectDto };
    A.CallTo(() => _subjectRepository.GetAllComplete()).Returns(subjects);

    var result = await _subjectService.GetAllComplete();

    result?.Should().BeOfType<List<ViewSubjectDto>>();

    result?.Should().BeSameAs(subjects);
  }

  [Fact]
  public async Task DeleteSubject_BDContainTheSubject_ShouldNotThrow()
  {
    var subjectId = _subject.Id;

    A.CallTo(() => _subjectRepository.GetById(subjectId))
      .Returns(Task.FromResult<Subject?>(_subject));
    A.CallTo(() => _subjectRepository.Remove(_subject)).Returns(Task.FromResult<bool>(true));

    Func<Task> act = async () => await _subjectService.Delete(subjectId);

    await act.Should().NotThrowAsync();
  }

  [Fact]
  public async Task DeleteSubject_NotFoundSubject_ShouldThrowWarning()
  {
    var subjectId = _subject.Id;

    A.CallTo(() => _subjectRepository.GetById(subjectId)).Returns(Task.FromResult<Subject?>(null));

    try
    {
      await _subjectService.Delete(subjectId);
    }
    catch (Exception ex)
    {
      ex.Should().BeOfType<NotFoundException>();
      ex.Message.Should().Be($"Subject id: {_subject.Id} not found");
    }
  }

  [Fact]
  public async Task PatchSubject_updateSucessfully_ShouldReturnSubject()
  {
    var updateSubjectDTO = new UpdateSubjectDto();
    var subjectId = _subject.Id;

    A.CallTo(() => _subjectRepository.GetById(subjectId))
      .Returns(Task.FromResult<Subject?>(_subject));
    A.CallTo(() => _subjectRepository.Edit(_subject)).Returns(Task.FromResult<bool>(true));

    var result = await _subjectService.Patch(subjectId, updateSubjectDTO);

    result.Should().Be(_subject);
  }

  [Fact]
  public async Task PatchSubject_NotFoundSubject_ShouldThrowNotFoundException()
  {
    var updateSubjectDTO = new UpdateSubjectDto();
    var quesitonId = _subject.Id;

    A.CallTo(() => _subjectRepository.GetById(quesitonId)).Returns(Task.FromResult<Subject?>(null));
    try
    {
      var result = await _subjectService.Patch(quesitonId, updateSubjectDTO);
    }
    catch (Exception ex)
    {
      ex.Should().BeOfType<NotFoundException>();
    }
  }

  [Fact]
  public async Task CreateSubject_ShouldReturnSubject_WhenCreateSubject()
  {
    var subjectDTO = new CreateSubjectDto() { Name = "Estruturas", AreaId = _studyArea.Id };

    A.CallTo(() => _areaService.GetById(_studyArea.Id)).Returns(Task.FromResult(_studyArea));
    A.CallTo(() => _subjectRepository.Add(A<Subject>._)).Returns(Task.FromResult(true));

    var result = await _subjectService.Create(subjectDTO);

    result?.Name.Should().Be(_subject.Name);
    result.Should().BeOfType<Subject>();
  }
}
