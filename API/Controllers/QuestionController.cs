using Apllication.DTOs;
using Apllication.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.Inharitance;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Persistence.Data.Repositories;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class QuestionController : ControllerBase
{
  private readonly ILogger<QuestionController> _logger;
  private readonly IQuestionRepository _questionRepository;
  private readonly IMapper _mapper;

  public QuestionController(
    ILogger<QuestionController> logger,
    IQuestionRepository questionRepository,
    IMapper mapper
  )
  {
    _logger = logger;
    _questionRepository = questionRepository;
    _mapper = mapper;
  }

  [HttpGet]
  public async Task<ActionResult> GetAllFull()
  {
    var questions = await _questionRepository.GetAllComplete();
    return Ok(questions);
  }

  [HttpGet(":id")]
  public async Task<ActionResult> GetFullById(int id)
  {
    var questions = await _questionRepository.GetFullById(id);
    return Ok(questions);
  }

  [HttpPost]
  public async Task<ActionResult> Create(CreateQuestionDTO questionDto)
  {
    BaseQuestion question;

    if (questionDto.Choices != null)
    {
      var choices = new List<Choice>();
      foreach (var choice in questionDto.Choices)
      {
        choices.Add(new Choice() { Letter = choice.Letter.ToCharArray()[0], Text = choice.Text });
      }
      question = new MultipleChoicesQuestion()
      {
        Body = questionDto.Body,
        Answer = questionDto.Answer.ToCharArray()[0],
        Tip = questionDto.Tip,
        CreatedAt = DateTime.UtcNow,
        LastUpdatedAt = DateTime.UtcNow,
        //CreatedBy = user,
        Choices = choices
      };
    }
    else
    {
      question = new BooleanQuestion()
      {
        Body = questionDto.Body,
        Answer = questionDto.Answer.ToCharArray()[0],
        Tip = questionDto.Tip,
        CreatedAt = DateTime.UtcNow,
        LastUpdatedAt = DateTime.UtcNow,
        //CreatedBy = user,
      };
    }

    _questionRepository.Add(question);
    if (await _questionRepository.SaveChanges())
    {
      var actionName = nameof(GetFullById);
      var routeValues = "";
      return CreatedAtAction(actionName, routeValues, question);
    }
    return BadRequest("Erro ao salvar a questão");
  }
}
