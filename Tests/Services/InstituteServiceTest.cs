using Apllication.DTOs.Institute;
using Apllication.Exceptions;
using Apllication.Repositories.Interfaces;
using Apllication.Services;
using Apllication.Services.Interfaces;
using Application.DTOs.Institute;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Tests;

public class InstituteServiceTest
{
  private readonly IInstituteRepository _instituteRepository;
  private readonly ILogger<IInstituteService> _logger;
  private readonly IInstituteService _areaService;
  private readonly Institute _institute =
    new()
    {
      Id = Guid.NewGuid(),
      Name = "Cebraspe",
      CreatedAt = new DateTime(2020, 07, 02, 22, 59, 59),
    };

  private ViewInstituteDto _viewInstituteDto { get; set; }

  public InstituteServiceTest()
  {
    _viewInstituteDto = new ViewInstituteDto
    {
      Id = _institute.Id,
      CreatedAt = _institute.CreatedAt,
      Name = _institute.Name,
    };
    _instituteRepository = A.Fake<IInstituteRepository>();
    _logger = A.Fake<ILogger<IInstituteService>>();
    _areaService = new InstituteService(_instituteRepository, _logger);
  }

  [Fact]
  public async Task GetInstitute_BDContainTheInstitute_ShouldReturnInstitute()
  {
    var id = _institute.Id;
    A.CallTo(() => _instituteRepository.GetById(id)).Returns(Task.FromResult(_institute));

    var result = await _areaService.GetById(id);

    result?.Should().BeOfType<Institute>();
    result?.Should().BeSameAs(_institute);
  }

  [Fact]
  public async Task GetAllInstitute_BDContainTheInstitute_ShouldReturnInstitutes()
  {
    var areas = new List<Institute> { _institute };
    A.CallTo(() => _instituteRepository.GetAll()).Returns(areas);

    var result = await _areaService.GetAll();

    result?.Should().BeOfType<List<Institute>>();

    result?.Should().BeSameAs(areas);
  }

  [Fact]
  public async Task DeleteInstitute_BDContainTheInstitute_ShouldNotThrow()
  {
    var areaId = _institute.Id;

    A.CallTo(() => _instituteRepository.GetById(areaId))
      .Returns(Task.FromResult<Institute?>(_institute));
    A.CallTo(() => _instituteRepository.Remove(_institute));
    A.CallTo(() => _instituteRepository.SaveChanges()).Returns(Task.FromResult<bool>(true));

    Func<Task> act = async () => await _areaService.Delete(areaId);

    await act.Should().NotThrowAsync();
  }

  [Fact]
  public async Task DeleteInstitute_NotFoundInstitute_ShouldThrowWarning()
  {
    var areaId = _institute.Id;

    A.CallTo(() => _instituteRepository.GetById(areaId)).Returns(Task.FromResult<Institute?>(null));

    try
    {
      await _areaService.Delete(areaId);
    }
    catch (Exception ex)
    {
      ex.Should().BeOfType<NotFoundException>();
      ex.Message.Should().Be($"Institute id: {_institute.Id} not found");
    }
  }

  [Fact]
  public async Task PatchInstitute_updateSucessfully_ShouldReturnInstitute()
  {
    var updateInstituteDTO = new UpdateInstituteDto();
    var areaId = _institute.Id;

    A.CallTo(() => _instituteRepository.GetById(areaId))
      .Returns(Task.FromResult<Institute?>(_institute));
    A.CallTo(() => _instituteRepository.SaveChanges()).Returns(Task.FromResult<bool>(true));

    var result = await _areaService.Patch(areaId, updateInstituteDTO);

    result.Should().Be(_institute);
  }

  [Fact]
  public async Task PatchInstitute_NotFoundInstitute_ShouldThrowNotFoundException()
  {
    var updateInstituteDTO = new UpdateInstituteDto();
    var quesitonId = _institute.Id;

    A.CallTo(() => _instituteRepository.GetById(quesitonId))
      .Returns(Task.FromResult<Institute?>(null));
    try
    {
      var result = await _areaService.Patch(quesitonId, updateInstituteDTO);
    }
    catch (Exception ex)
    {
      ex.Should().BeOfType<NotFoundException>();
    }
  }

  [Fact]
  public async Task CreateInstitute_ShouldReturnMultipleChoiceInstitute_WhenCreateInstituteWithChoices()
  {
    var areaDTO = new CreateInstituteDto() { Name = "Cebraspe", };

    A.CallTo(() => _instituteRepository.Add(_institute));
    A.CallTo(() => _instituteRepository.SaveChanges()).Returns(Task.FromResult<bool>(true));

    var result = await _areaService.Create(areaDTO);

    result?.Name.Should().Be(_institute.Name);
    result.Should().BeOfType<Institute>();
  }

  [Fact]
  public async Task CreateInstitute_ShouldReturnBooleanInstitute_WhenCreateInstituteWithoutChoices()
  {
    var areaDTO = new CreateInstituteDto() { Name = "Cebraspe", };

    A.CallTo(() => _instituteRepository.Add(_institute));
    A.CallTo(() => _instituteRepository.SaveChanges()).Returns(Task.FromResult<bool>(true));

    var result = await _areaService.Create(areaDTO);

    result?.Name.Should().Be(_institute.Name);
    result.Should().BeOfType<Institute>();
  }
}
