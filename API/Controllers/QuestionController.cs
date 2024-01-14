using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class QuestionController : ControllerBase
{

    private readonly ILogger<QuestionController> _logger;
    private readonly DataContext _dataContextEF;

    public QuestionController(
        ILogger<QuestionController> logger,
        DataContext dataContextEF
        )
    {
        _logger = logger;
        _dataContextEF = dataContextEF;
    }

    [HttpGet]
    public async Task<ActionResult> Get()
    {
        var questions = await _dataContextEF.Questions.ToListAsync();
        return Ok(questions);
    }
}
