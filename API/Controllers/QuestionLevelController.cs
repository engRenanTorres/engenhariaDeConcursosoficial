using API.Extensions.HttpExtensions;
using Apllication.Core;
using Apllication.DTO;
using Apllication.DTOs;
using Apllication.DTOs.QLevel;
using Apllication.Services.Interfaces;
using Application.DTOs;
using Application.DTOs.QLevel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuestionLevelController : ControllerBase
{
  private readonly ILogger<QuestionLevelController> _logger;
  private readonly IQLevelService _questionLevelService;

  public QuestionLevelController(
    ILogger<QuestionLevelController> logger,
    IQLevelService questionService
  )
  {
    _logger = logger;
    _questionLevelService = questionService;
  }

  [HttpGet]
  [AllowAnonymous]
  public async Task<ActionResult> GetAll()
  {
    var questionLevels = await _questionLevelService.GetAll();
    return Ok(questionLevels);
  }

  [HttpGet]
  [AllowAnonymous]
  [Route("Paged")]
  public async Task<ActionResult> GetAllFull([FromQuery] PagingParams pagingParams)
  {
    var questionLevels = await _questionLevelService.GetAllPaged(pagingParams);
    Response.AddPaginationHeader(
      questionLevels.CurrentPage,
      questionLevels.PageSize,
      questionLevels.TotalCount,
      questionLevels.TotalPages
    );
    return Ok(questionLevels);
  }

  [HttpGet("{id}")]
  public async Task<ActionResult> GetFullById(Guid id)
  {
    var questionLevels = await _questionLevelService.GetById(id);
    return Ok(questionLevels);
  }

  [Authorize(Roles = "Admin, Member")]
  [HttpPost]
  public async Task<ActionResult> Create(CreateQLevelDto dto)
  {
    var questionLevel = await _questionLevelService.Create(dto);
    var actionName = nameof(GetFullById);
    return CreatedAtAction(actionName, new { Id = "LastIdCreated" }, questionLevel);
  }

  [HttpDelete("{id}")]
  public async Task<ActionResult> Delete(Guid id)
  {
    _logger.LogInformation("Delete Controller has been called.");

    await _questionLevelService.Delete(id);

    return NoContent();
  }

  [HttpPatch("{id}")]
  public async Task<ActionResult> Patch([FromBody] UpdateQLevelDto dto)
  {
    _logger.LogInformation("Patch has been called.");

    var updatedQuestionLevel = await _questionLevelService.Patch(dto.Id, dto);

    return Ok(updatedQuestionLevel);
  }
}
