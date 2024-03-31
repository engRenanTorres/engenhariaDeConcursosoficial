using API.Extensions.HttpExtensions;
using Apllication.Core;
using Apllication.DTO;
using Apllication.DTOs;
using Apllication.Services.Interfaces;
using Application.DTOs;
using Domain.Entities;
using Domain.Entities.Questions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuestionController : ControllerBase
{
  private readonly ILogger<QuestionController> _logger;
  private readonly IQuestionService _questionService;

  public QuestionController(ILogger<QuestionController> logger, IQuestionService questionService)
  {
    _logger = logger;
    _questionService = questionService;
  }

  [HttpGet]
  [AllowAnonymous]
  public async Task<ActionResult> GetAllFull([FromQuery] QuestionParams pagingParams)
  {
    var questions = await _questionService.GetAllComplete(pagingParams);
    Response.AddPaginationHeader(
      questions.CurrentPage,
      questions.PageSize,
      questions.TotalCount,
      questions.TotalPages
    );
    return Ok(questions);
  }

  [HttpGet("{id}")]
  [AllowAnonymous]
  public async Task<ActionResult> GetFullById(int id)
  {
    var questions = await _questionService.GetFullById(id);
    return Ok(questions);
  }

  [HttpGet("LastId")]
  [AllowAnonymous]
  public async Task<ActionResult> GetCount()
  {
    int id = await _questionService.GetCount();
    return Ok(id);
  }

  [Authorize(Roles = "Admin")]
  [HttpPost]
  public async Task<ActionResult> Create(CreateQuestionDTO questionDto)
  {
    Question question = await _questionService.Create(questionDto);
    var actionName = nameof(GetFullById);
    var LastIdCreated = await _questionService.GetLastId();
    var userDto = new UserDto()
    {
      DisplayName = question.InsertedBy?.DisplayName ?? "",
      Id = question.InsertedBy?.Id ?? "",
    };
    ViewQuestionDto viewQuestion = ParseToView(question, LastIdCreated, userDto);
    return CreatedAtAction(actionName, new { Id = LastIdCreated }, viewQuestion);
  }

  private static ViewQuestionDto ParseToView(Question question, int LastIdCreated, UserDto userDto)
  {
    var jsonQuestion = new ViewQuestionDto()
    {
      Id = LastIdCreated,
      Answer = (question as ChoicesQuestion)?.Answer ?? char.Parse(""),
      Body = question.Body,
      //EditedBy = userDto,
      InsertedAt = question.InsertedAt,
      InsertedBy = userDto,
      LastUpdatedAt = question.LastUpdatedAt,
      Tip = question.Tip,
      Choices = (question as ChoicesQuestion)?.Choices ?? new List<Choice>(),
      Concurso = new() { Name = question.Concurso?.Name ?? "", },
      Subject = question.Subject?.Name ?? "",
      StudyArea = question.Subject?.StudyArea?.Name ?? "",
      Level = question.QuestionLevel?.Name ?? ""
    };
    return jsonQuestion;
  }

  [Authorize(Roles = "Admin")]
  [HttpDelete("{id}")]
  public async Task<ActionResult> DeleteQuestion(int id)
  {
    _logger.LogInformation("Delete Question Controller has been called.");

    await _questionService.Delete(id);

    return NoContent();
  }

  [Authorize(Roles = "Admin")]
  [HttpPatch("{id}")]
  public async Task<ActionResult> PatchQuestion(
    int id,
    [FromBody] UpdateQuestionDTO updateQuestionDTO
  )
  {
    _logger.LogInformation("PatchQuestions has been called.");

    var updatedQuestion = await _questionService.Patch(id, updateQuestionDTO);

    return Ok(updatedQuestion);
  }
}
