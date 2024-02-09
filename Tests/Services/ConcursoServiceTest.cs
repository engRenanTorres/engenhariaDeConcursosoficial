using Apllication.DTOs.Concurso;
using Apllication.Exceptions;
using Apllication.Repositories.Interfaces;
using Apllication.Services;
using Apllication.Services.Interfaces;
using Application.DTOs.Concurso;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Tests;

public class ConcursoServiceTest
{
  private readonly IConcursoRepository _concursoRepository;
  private readonly ILogger<IConcursoService> _logger;
  private readonly IConcursoService _areaService;
  private readonly IInstituteService _instituteService;
  private Concurso _concurso;

  private readonly Institute _institute =
    new()
    {
      Id = Guid.NewGuid(),
      Name = "Cebraspe",
      CreatedAt = new DateTime(2020, 07, 02, 22, 59, 59),
    };

  public ConcursoServiceTest()
  {
    _concurso = new Concurso
    {
      Id = Guid.NewGuid(),
      CreatedAt = new DateTime(2020, 07, 02, 22, 59, 59),
      Name = "Petrobras",
      Institute = _institute,
    };
    _concursoRepository = A.Fake<IConcursoRepository>();
    _logger = A.Fake<ILogger<IConcursoService>>();
    _instituteService = A.Fake<InstituteService>();
    _areaService = new ConcursoService(_concursoRepository, _instituteService, _logger);
  }

  [Fact]
  public async Task GetConcurso_BDContainTheConcurso_ShouldReturnConcurso()
  {
    var id = _concurso.Id;
    A.CallTo(() => _concursoRepository.GetById(id)).Returns(Task.FromResult(_concurso));

    var result = await _areaService.GetById(id);

    result?.Should().BeOfType<Concurso>();
    result?.Should().BeSameAs(_concurso);
  }

  [Fact]
  public async Task GetAllConcurso_BDContainTheConcurso_ShouldReturnConcursos()
  {
    var areas = new List<Concurso> { _concurso };
    A.CallTo(() => _concursoRepository.GetAll()).Returns(areas);

    var result = await _areaService.GetAll();

    result?.Should().BeOfType<List<Concurso>>();

    result?.Should().BeSameAs(areas);
  }

  [Fact]
  public async Task DeleteConcurso_BDContainTheConcurso_ShouldNotThrow()
  {
    var areaId = _concurso.Id;

    A.CallTo(() => _concursoRepository.GetById(areaId))
      .Returns(Task.FromResult<Concurso?>(_concurso));
    A.CallTo(() => _concursoRepository.Remove(_concurso));
    A.CallTo(() => _concursoRepository.SaveChanges()).Returns(Task.FromResult<bool>(true));

    Func<Task> act = async () => await _areaService.Delete(areaId);

    await act.Should().NotThrowAsync();
  }

  [Fact]
  public async Task DeleteConcurso_NotFoundConcurso_ShouldThrowWarning()
  {
    var areaId = _concurso.Id;

    A.CallTo(() => _concursoRepository.GetById(areaId)).Returns(Task.FromResult<Concurso?>(null));

    try
    {
      await _areaService.Delete(areaId);
    }
    catch (Exception ex)
    {
      ex.Should().BeOfType<NotFoundException>();
      ex.Message.Should().Be($"Concurso id: {_concurso.Id} not found");
    }
  }

  [Fact]
  public async Task PatchConcurso_updateSucessfully_ShouldReturnConcurso()
  {
    var updateConcursoDTO = new UpdateConcursoDto();
    var areaId = _concurso.Id;

    A.CallTo(() => _concursoRepository.GetById(areaId))
      .Returns(Task.FromResult<Concurso?>(_concurso));
    A.CallTo(() => _concursoRepository.SaveChanges()).Returns(Task.FromResult<bool>(true));

    var result = await _areaService.Patch(areaId, updateConcursoDTO);

    result.Should().Be(_concurso);
  }

  [Fact]
  public async Task PatchConcurso_NotFoundConcurso_ShouldThrowNotFoundException()
  {
    var updateConcursoDTO = new UpdateConcursoDto();
    var quesitonId = _concurso.Id;

    A.CallTo(() => _concursoRepository.GetById(quesitonId))
      .Returns(Task.FromResult<Concurso?>(null));
    try
    {
      var result = await _areaService.Patch(quesitonId, updateConcursoDTO);
    }
    catch (Exception ex)
    {
      ex.Should().BeOfType<NotFoundException>();
    }
  }

  [Fact]
  public async Task CreateConcurso_ShouldReturnMultipleChoiceConcurso_WhenCreateConcursoWithChoices()
  {
    var areaDTO = new CreateConcursoDto() { Name = "Petrobras", };

    A.CallTo(() => _concursoRepository.Add(_concurso));
    A.CallTo(() => _concursoRepository.SaveChanges()).Returns(Task.FromResult<bool>(true));

    var result = await _areaService.Create(areaDTO);

    result?.Name.Should().Be(_concurso.Name);
    result.Should().BeOfType<Concurso>();
  }

  [Fact]
  public async Task CreateConcurso_ShouldReturnBooleanConcurso_WhenCreateConcursoWithoutChoices()
  {
    var areaDTO = new CreateConcursoDto() { Name = "Petrobras", };

    A.CallTo(() => _concursoRepository.Add(_concurso));
    A.CallTo(() => _concursoRepository.SaveChanges()).Returns(Task.FromResult<bool>(true));

    var result = await _areaService.Create(areaDTO);

    result?.Name.Should().Be(_concurso.Name);
    result.Should().BeOfType<Concurso>();
  }
}
