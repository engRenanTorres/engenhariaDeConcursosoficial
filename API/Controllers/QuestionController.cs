using Apllication.DTOs;
using Apllication.Services.Interfaces;
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
}
