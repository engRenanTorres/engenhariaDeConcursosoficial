using Apllication.DTOs.Concurso;
using Apllication.Services.Interfaces;
using Application.DTOs;
using Application.DTOs.Concurso;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConcursoController : ControllerBase
{
  private readonly ILogger<ConcursoController> _logger;
  private readonly IConcursoService _concursoService;

  public ConcursoController(ILogger<ConcursoController> logger, IConcursoService concursoService)
  {
    _logger = logger;
    _concursoService = concursoService;
  }

  [HttpGet]
  [AllowAnonymous]
  public async Task<ActionResult> GetAll()
  {
    var concursos = await _concursoService.GetAll();
    return Ok(concursos);
  }

  // [HttpGet]
  // [AllowAnonymous]
  // [Route("Paged")]
  // public async Task<ActionResult> GetAllFull([FromQuery] PagingParams pagingParams)
  // {
  //   var concursos = await _concursoService.GetAllPaged(pagingParams);
  //   Response.AddPaginationHeader(
  //     concursos.CurrentPage,
  //     concursos.PageSize,
  //     concursos.TotalCount,
  //     concursos.TotalPages
  //   );
  //   return Ok(concursos);
  // }

  [HttpGet("{id}")]
  public async Task<ActionResult> GetFullById(Guid id)
  {
    var concursos = await _concursoService.GetById(id);
    return Ok(concursos);
  }

  [Authorize(Roles = "Admin, Member")]
  [HttpPost]
  public async Task<ActionResult> Create(CreateConcursoDto dto)
  {
    var concurso = await _concursoService.Create(dto);
    var actionName = nameof(GetFullById);
    return CreatedAtAction(actionName, new { Id = "LastIdCreated" }, concurso);
  }

  [HttpDelete("{id}")]
  public async Task<ActionResult> Delete(Guid id)
  {
    _logger.LogInformation("Delete Controller has been called.");

    await _concursoService.Delete(id);

    return NoContent();
  }

  [HttpPatch("{id}")]
  public async Task<ActionResult> Patch([FromBody] UpdateConcursoDto dto)
  {
    _logger.LogInformation("Patch has been called.");

    var updatedConcurso = await _concursoService.Patch(dto.Id, dto);

    return Ok(updatedConcurso);
  }
}
