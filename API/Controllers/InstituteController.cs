using Apllication.DTOs.Institute;
using Apllication.Services.Interfaces;
using Application.DTOs;
using Application.DTOs.Institute;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InstituteController : ControllerBase
{
  private readonly ILogger<InstituteController> _logger;
  private readonly IInstituteService _instituteService;

  public InstituteController(ILogger<InstituteController> logger, IInstituteService areaService)
  {
    _logger = logger;
    _instituteService = areaService;
  }

  [HttpGet]
  [AllowAnonymous]
  public async Task<ActionResult> GetAll()
  {
    var institutes = await _instituteService.GetAll();
    return Ok(institutes);
  }

  // [HttpGet]
  // [AllowAnonymous]
  // [Route("Paged")]
  // public async Task<ActionResult> GetAllFull([FromQuery] PagingParams pagingParams)
  // {
  //   var institutes = await _instituteService.GetAllPaged(pagingParams);
  //   Response.AddPaginationHeader(
  //     institutes.CurrentPage,
  //     institutes.PageSize,
  //     institutes.TotalCount,
  //     institutes.TotalPages
  //   );
  //   return Ok(institutes);
  // }

  [HttpGet("{id}")]
  public async Task<ActionResult> GetFullById(Guid id)
  {
    var institutes = await _instituteService.GetById(id);
    return Ok(institutes);
  }

  [Authorize(Roles = "Admin, Member")]
  [HttpPost]
  public async Task<ActionResult> Create(CreateInstituteDto dto)
  {
    var institute = await _instituteService.Create(dto);
    var actionName = nameof(GetFullById);
    return CreatedAtAction(actionName, new { Id = "LastIdCreated" }, institute);
  }

  [HttpDelete("{id}")]
  public async Task<ActionResult> Delete(Guid id)
  {
    _logger.LogInformation("Delete Controller has been called.");

    await _instituteService.Delete(id);

    return NoContent();
  }

  [HttpPatch("{id}")]
  public async Task<ActionResult> Patch([FromBody] UpdateInstituteDto dto)
  {
    _logger.LogInformation("Patch has been called.");

    var updatedInstitute = await _instituteService.Patch(dto.Id, dto);

    return Ok(updatedInstitute);
  }
}
