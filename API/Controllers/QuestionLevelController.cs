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
  private readonly ILogger<QuestionController> _logger;
  private readonly IQLevelService _questionLevelService;

  public QuestionLevelController(ILogger<QuestionController> logger, IQLevelService questionService)
  {
    _logger = logger;
    _questionLevelService = questionService;
  }

  [HttpGet]
  [AllowAnonymous]
  public async Task<ActionResult> GetAll()
  {
    var questions = await _questionLevelService.GetAll();
    return Ok(questions);
  }

  [HttpGet]
  [AllowAnonymous]
  [Route("Paged")]
  public async Task<ActionResult> GetAllFull([FromQuery] QuestionParams pagingParams)
  {
    var questions = await _questionLevelService.GetAllPaged(pagingParams);
    Response.AddPaginationHeader(
      questions.CurrentPage,
      questions.PageSize,
      questions.TotalCount,
      questions.TotalPages
    );
    return Ok(questions);
  }

  [HttpGet("{id}")]
  public async Task<ActionResult> GetFullById(Guid id)
  {
    var questions = await _questionLevelService.GetById(id);
    return Ok(questions);
  }

  [Authorize(Roles = "Admin, Member")]
  [HttpPost]
  public async Task<ActionResult> Create(CreateQLevelDto dto)
  {
    var question = await _questionLevelService.Create(dto);
    var actionName = nameof(GetFullById);
    return CreatedAtAction(actionName, new { Id = "LastIdCreated" }, question);
  }

  [HttpDelete("{id}")]
  public async Task<ActionResult> Delete(Guid id)
  {
    _logger.LogInformation("Delete Controller has been called.");

    await _questionLevelService.Delete(id);

    return NoContent();
  }

  [HttpPatch("{id}")]
  public async Task<ActionResult> PatchQuestion([FromBody] UpdateQLevelDto dto)
  {
    _logger.LogInformation("Patch has been called.");

    var updatedQuestion = await _questionLevelService.Patch(dto.Id, dto);

    return Ok(updatedQuestion);
  }
}
