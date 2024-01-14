using Apllication.Repositories;
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
    public QuestionController(
        ILogger<QuestionController> logger,
        IQuestionRepository questionRepository
        )
    {
        _logger = logger;
        _questionRepository = questionRepository;
    }

    [HttpGet]
    public async Task<ActionResult> Get()
    {
        var questions = await _questionRepository.GetAllMultiple();
        return Ok(questions);
    }
}
