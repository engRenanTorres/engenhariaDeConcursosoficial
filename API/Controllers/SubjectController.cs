using API.Extensions.HttpExtensions;
using Apllication.Core;
using Apllication.DTO;
using Apllication.DTOs;
using Apllication.DTOs.QLevel;
using Apllication.Services.Interfaces;
using Application.DTOs;
using Application.DTOs.QLevel;
using Application.DTOs.SubjectDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SubjectController : ControllerBase
{
  private readonly ILogger<SubjectController> _logger;
  private readonly ISubjectService _subjectService;

  public SubjectController(ILogger<SubjectController> logger, ISubjectService areaService)
  {
    _logger = logger;
    _subjectService = areaService;
  }

  [HttpGet]
  [AllowAnonymous]
  public async Task<ActionResult> GetAll()
  {
    var subjects = await _subjectService.GetAllComplete();
    return Ok(subjects);
  }

  // [HttpGet]
  // [AllowAnonymous]
  // [Route("Paged")]
  // public async Task<ActionResult> GetAllFull([FromQuery] PagingParams pagingParams)
  // {
  //   var subjects = await _subjectService.GetAllPaged(pagingParams);
  //   Response.AddPaginationHeader(
  //     subjects.CurrentPage,
  //     subjects.PageSize,
  //     subjects.TotalCount,
  //     subjects.TotalPages
  //   );
  //   return Ok(subjects);
  // }

  [HttpGet("{id}")]
  public async Task<ActionResult> GetFullById(Guid id)
  {
    var subjects = await _subjectService.GetById(id);
    return Ok(subjects);
  }

  [Authorize(Roles = "Admin, Member")]
  [HttpPost]
  public async Task<ActionResult> Create(CreateSubjectDto dto)
  {
    var subject = await _subjectService.Create(dto);
    var actionName = nameof(GetFullById);
    return CreatedAtAction(actionName, new { Id = "LastIdCreated" }, subject);
  }

  [HttpDelete("{id}")]
  public async Task<ActionResult> Delete(Guid id)
  {
    _logger.LogInformation("Delete Controller has been called.");

    await _subjectService.Delete(id);

    return NoContent();
  }

  [HttpPatch("{id}")]
  public async Task<ActionResult> Patch([FromBody] UpdateSubjectDto dto)
  {
    _logger.LogInformation("Patch has been called.");

    var updatedSubject = await _subjectService.Patch(dto.Id, dto);

    return Ok(updatedSubject);
  }
}
