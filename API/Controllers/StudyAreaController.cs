using API.Extensions.HttpExtensions;
using Apllication.Core;
using Apllication.DTO;
using Apllication.DTOs;
using Apllication.DTOs.QLevel;
using Apllication.DTOs.StudyArea;
using Apllication.Services.Interfaces;
using Application.DTOs;
using Application.DTOs.QLevel;
using Application.DTOs.StudyArea;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudyAreaController : ControllerBase
{
  private readonly ILogger<StudyAreaController> _logger;
  private readonly IAreaService _studyAreaService;

  public StudyAreaController(ILogger<StudyAreaController> logger, IAreaService areaService)
  {
    _logger = logger;
    _studyAreaService = areaService;
  }

  [HttpGet]
  [AllowAnonymous]
  public async Task<ActionResult> GetAll()
  {
    var areas = await _studyAreaService.GetAll();
    return Ok(areas);
  }

  // [HttpGet]
  // [AllowAnonymous]
  // [Route("Paged")]
  // public async Task<ActionResult> GetAllFull([FromQuery] PagingParams pagingParams)
  // {
  //   var areas = await _studyAreaService.GetAllPaged(pagingParams);
  //   Response.AddPaginationHeader(
  //     areas.CurrentPage,
  //     areas.PageSize,
  //     areas.TotalCount,
  //     areas.TotalPages
  //   );
  //   return Ok(areas);
  // }

  [HttpGet("{id}")]
  public async Task<ActionResult> GetFullById(Guid id)
  {
    var areas = await _studyAreaService.GetById(id);
    return Ok(areas);
  }

  [Authorize(Roles = "Admin, Member")]
  [HttpPost]
  public async Task<ActionResult> Create(CreateAreaDto dto)
  {
    var area = await _studyAreaService.Create(dto);
    var actionName = nameof(GetFullById);
    return CreatedAtAction(actionName, new { Id = "LastIdCreated" }, area);
  }

  [HttpDelete("{id}")]
  public async Task<ActionResult> Delete(Guid id)
  {
    _logger.LogInformation("Delete Controller has been called.");

    await _studyAreaService.Delete(id);

    return NoContent();
  }

  [HttpPatch("{id}")]
  public async Task<ActionResult> Patch([FromBody] UpdateAreaDto dto)
  {
    _logger.LogInformation("Patch has been called.");

    var updatedArea = await _studyAreaService.Patch(dto.Id, dto);

    return Ok(updatedArea);
  }
}
