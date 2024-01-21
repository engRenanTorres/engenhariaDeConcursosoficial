using Apllication.DTOs;
using Apllication.Services.Interfaces;
using Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
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
  public async Task<ActionResult> GetAllFull()
  {
    var questions = await _questionService.GetAllComplete();
    return Ok(questions);
  }

  [HttpGet("{id}")]
  public async Task<ActionResult> GetFullById(int id)
  {
    var questions = await _questionService.GetFullById(id);
    return Ok(questions);
  }

  [HttpGet("LastId")]
  public async Task<ActionResult> GetCount()
  {
    int id = await _questionService.GetCount();
    return Ok(id);
  }

  [HttpPost]
  public async Task<ActionResult> Create(CreateQuestionDTO questionDto)
  {
    var question = await _questionService.Create(questionDto);
    var actionName = nameof(GetFullById);
    var LastIdCreated = await _questionService.GetLastId();
    return CreatedAtAction(actionName, new { Id = LastIdCreated }, question);
  }

  [HttpDelete("{id}")]
  public async Task<ActionResult> DeleteQuestion(int id)
  {
    _logger.LogInformation("Delete Question Controller has been called.");

    await _questionService.Delete(id);

    return NoContent();
  }

  [HttpPatch("{id}")]
  public async Task<ActionResult> PatchQuestion(
    int id,
    [FromBody] UpdateQuestionDTO updateQuestionDTO
  )
  {
    _logger.LogInformation("PatchQuestions has been called.");

    var updatedQuestion = await _questionService.PatchQuestion(id, updateQuestionDTO);

    return Ok(updatedQuestion);
  }
}
