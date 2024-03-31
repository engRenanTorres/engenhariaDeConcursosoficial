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
  private readonly IConcursoService _concursoService;
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
    _instituteService = A.Fake<IInstituteService>();
    _concursoService = new ConcursoService(_concursoRepository, _instituteService, _logger);
  }

  [Fact]
  public async Task GetConcurso_BDContainTheConcurso_ShouldReturnConcurso()
  {
    var id = _concurso.Id;
    A.CallTo(() => _concursoRepository.GetById(id)).Returns(Task.FromResult(_concurso));

    var result = await _concursoService.GetById(id);

    result?.Should().BeOfType<Concurso>();
    result?.Should().BeSameAs(_concurso);
  }

  [Fact]
  public async Task GetAllConcurso_BDContainTheConcurso_ShouldReturnConcursos()
  {
    var concursos = new List<Concurso> { _concurso };
    A.CallTo(() => _concursoRepository.GetAll()).Returns(concursos);

    var result = await _concursoService.GetAll();

    result?.Should().BeOfType<List<Concurso>>();

    result?.Should().BeSameAs(concursos);
  }

  [Fact]
  public async Task DeleteConcurso_BDContainTheConcurso_ShouldNotThrow()
  {
    var concursoId = _concurso.Id;

    A.CallTo(() => _concursoRepository.GetById(concursoId))
      .Returns(Task.FromResult<Concurso?>(_concurso));
    A.CallTo(() => _concursoRepository.Remove(_concurso)).Returns(Task.FromResult<bool>(true));

    Func<Task> act = async () => await _concursoService.Delete(concursoId);

    await act.Should().NotThrowAsync();
  }

  [Fact]
  public async Task DeleteConcurso_NotFoundConcurso_ShouldThrowWarning()
  {
    var concursoId = _concurso.Id;

    A.CallTo(() => _concursoRepository.GetById(concursoId))
      .Returns(Task.FromResult<Concurso?>(null));

    try
    {
      await _concursoService.Delete(concursoId);
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
    var concursoId = _concurso.Id;

    A.CallTo(() => _concursoRepository.GetById(concursoId))
      .Returns(Task.FromResult<Concurso?>(_concurso));
    A.CallTo(() => _concursoRepository.Edit(_concurso)).Returns(Task.FromResult<bool>(true));

    var result = await _concursoService.Patch(concursoId, updateConcursoDTO);

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
      var result = await _concursoService.Patch(quesitonId, updateConcursoDTO);
    }
    catch (Exception ex)
    {
      ex.Should().BeOfType<NotFoundException>();
    }
  }

  [Fact]
  public async Task CreateConcurso_ShouldReturnConcurso_WhenCreateConcurso()
  {
    var concursoDTO = new CreateConcursoDto() { Name = "Petrobras", };

    A.CallTo(() => _instituteService.GetById(_institute.Id)).Returns(Task.FromResult(_institute));
    A.CallTo(() => _concursoRepository.Add(A<Concurso>._)).Returns(Task.FromResult<bool>(true));

    var result = await _concursoService.Create(concursoDTO);

    result?.Name.Should().Be(_concurso.Name);
    result.Should().BeOfType<Concurso>();
  }
}
