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
  private readonly IInstituteService _instituteService;
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
    _instituteService = new InstituteService(_instituteRepository, _logger);
  }

  [Fact]
  public async Task GetInstitute_BDContainTheInstitute_ShouldReturnInstitute()
  {
    var id = _institute.Id;
    A.CallTo(() => _instituteRepository.GetById(id)).Returns(Task.FromResult(_institute));

    var result = await _instituteService.GetById(id);

    result?.Should().BeOfType<Institute>();
    result?.Should().BeSameAs(_institute);
  }

  [Fact]
  public async Task GetAllInstitute_BDContainTheInstitute_ShouldReturnInstitutes()
  {
    var institutes = new List<Institute> { _institute };
    A.CallTo(() => _instituteRepository.GetAll()).Returns(institutes);

    var result = await _instituteService.GetAll();

    result?.Should().BeOfType<List<Institute>>();

    result?.Should().BeSameAs(institutes);
  }

  [Fact]
  public async Task DeleteInstitute_BDContainTheInstitute_ShouldNotThrow()
  {
    var instituteId = _institute.Id;

    A.CallTo(() => _instituteRepository.GetById(instituteId))
      .Returns(Task.FromResult<Institute?>(_institute));
    A.CallTo(() => _instituteRepository.Remove(_institute)).Returns(Task.FromResult<bool>(true));

    Func<Task> act = async () => await _instituteService.Delete(instituteId);

    await act.Should().NotThrowAsync();
  }

  [Fact]
  public async Task DeleteInstitute_NotFoundInstitute_ShouldThrowWarning()
  {
    var instituteId = _institute.Id;

    A.CallTo(() => _instituteRepository.GetById(instituteId))
      .Returns(Task.FromResult<Institute?>(null));

    try
    {
      await _instituteService.Delete(instituteId);
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
    var instituteId = _institute.Id;

    A.CallTo(() => _instituteRepository.GetById(instituteId))
      .Returns(Task.FromResult<Institute?>(_institute));
    A.CallTo(() => _instituteRepository.Edit(_institute)).Returns(Task.FromResult<bool>(true));

    var result = await _instituteService.Patch(instituteId, updateInstituteDTO);

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
      var result = await _instituteService.Patch(quesitonId, updateInstituteDTO);
    }
    catch (Exception ex)
    {
      ex.Should().BeOfType<NotFoundException>();
    }
  }

  [Fact]
  public async Task CreateInstitute_ShouldReturnInstitute_WhenCreateInstitute()
  {
    var instituteDTO = new CreateInstituteDto() { Name = "Cebraspe", };

    A.CallTo(() => _instituteRepository.Add(A<Institute>._)).Returns(Task.FromResult<bool>(true));

    var result = await _instituteService.Create(instituteDTO);

    result?.Name.Should().Be(_institute.Name);
    result.Should().BeOfType<Institute>();
  }
}
