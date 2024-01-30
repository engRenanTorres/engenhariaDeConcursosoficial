using System.ComponentModel;
using System.Security.Claims;
using API.Controllers;
using Apllication.DTOs;
using Apllication.Exceptions;
using Apllication.Services.Interfaces;
using Application.DTOs;
using Domain.Entities;
using Domain.Entities.Inharitance;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DotnetAPITests.Controllers
{
  public class QuestionControllerTest
  {
    private readonly IQuestionService _questionService;
    private readonly ILogger<QuestionController> _logger;
    private readonly QuestionController _questionController;

    private readonly AppUser _appUser =
      new()
      {
        DisplayName = "TestUser",
        Email = "test@tes.com",
        UserName = "tester"
      };
    private readonly MultipleChoicesQuestion _question =
      new()
      {
        Id = 1,
        CreatedAt = new DateTime(2020, 07, 02, 22, 59, 59),
        LastUpdatedAt = new DateTime(2020, 07, 02, 22, 59, 59),
        Body = "Is this a quetion Test?",
        Answer = 'A',
        Tip = "A",
      };

    private ViewQuestionDto _viewQuestionDto { get; set; }

    public QuestionControllerTest()
    {
      _question.CreatedBy = _appUser;
      _viewQuestionDto = new ViewQuestionDto
      {
        Id = _question.Id,
        CreatedAt = _question.CreatedAt,
        LastUpdatedAt = _question.LastUpdatedAt,
        Body = _question.Body,
        Answer = _question.Answer,
        Tip = _question.Tip,
        CreatedBy = new UserDto
        {
          DisplayName = _question.CreatedBy.DisplayName,
          Username = _question.CreatedBy.UserName,
        }
      };
      _questionService = A.Fake<IQuestionService>();
      _logger = A.Fake<ILogger<QuestionController>>();
      _questionController = new QuestionController(_logger, _questionService);
    }

    [Fact]
    public async Task CreateQuestion_ReturnQuetion()
    {
      var newQuestion = new CreateQuestionDTO()
      {
        Body = "This is a quetion Test?",
        Answer = "A",
        Tip = "A",
      };
      var authUserId = "1";
      var httpContext = new DefaultHttpContext();
      var userClaims = new ClaimsPrincipal(
        new ClaimsIdentity(new Claim[] { new Claim("UserId", authUserId) })
      );
      httpContext.User = userClaims;
      _questionController.ControllerContext.HttpContext = httpContext;

      A.CallTo(() => _questionService.Create(newQuestion))
        .Returns(Task.FromResult<BaseQuestion?>(_question));

      var result = await _questionController.Create(newQuestion);

      result.Should().BeOfType<CreatedAtActionResult>();
      var createdResult = result as CreatedAtActionResult;

      createdResult?.StatusCode.Should().Be(StatusCodes.Status201Created);

      createdResult?.Value.Should().BeSameAs(_question);
    }

    /*[Fact]
    public async Task CreateQuestion_InvalidUserId_ThrowsNotFound()
    {
      var createQuestionDTO = new CreateQuestionDTO();
      var actionResult = await _questionController.Create(createQuestionDTO);

      actionResult.Should().NotBeNull();
      actionResult.Should().BeOfType<NotFoundObjectResult>();
      var badRequestResult = actionResult as NotFoundObjectResult;

      badRequestResult?.StatusCode.Should().Be(404);

      badRequestResult?.Value.Should().Be("Please log a user");
    }*/

    /*[Fact]
    public async Task CreateQuestion_InvalidData_ThrowsBadRequest()
    {
      var authUserId = "1";
      var httpContext = new DefaultHttpContext();
      var userClaims = new ClaimsPrincipal(
        new ClaimsIdentity(new Claim[] { new Claim("UserId", authUserId) })
      );
      httpContext.User = userClaims;
      _questionController.ControllerContext.HttpContext = httpContext;
      var createQuestionDTO = new CreateQuestionDTO();

      A.CallTo(() => _questionService.Create(createQuestionDTO))
        .Returns(Task.FromResult<BaseQuestion?>(null));

      var actionResult = await _questionController.Create(createQuestionDTO);

      actionResult.Should().NotBeNull();
      actionResult.Should().BeOfType<BadRequestObjectResult>();
      var badRequestResult = actionResult as BadRequestObjectResult;

      badRequestResult?.StatusCode.Should().Be(400);

      badRequestResult?.Value.Should().Be("Question has Not been Created");
    }*/

    [Fact]
    public async Task GetQuestion_ReturnQuestion()
    {
      A.CallTo(() => _questionService.GetFullById(1))
        .Returns(Task.FromResult<ViewQuestionDto?>(_viewQuestionDto));

      var result = await _questionController.GetFullById(1);

      result.Should().BeOfType<OkObjectResult>();
      var createdResult = result as OkObjectResult;

      createdResult?.StatusCode.Should().Be(StatusCodes.Status200OK);

      createdResult?.Value.Should().BeEquivalentTo(_viewQuestionDto);
    }

    [Fact]
    public async Task GetQuestion_NotFoundQuestion_ThrowsNotFound()
    {
      var exceptionMessage = "Question id: 1 not found";
      A.CallTo(() => _questionService.GetFullById(1))
        .Throws(new NotFoundException(exceptionMessage));

      try
      {
        var actionResult = await _questionController.GetFullById(1);
      }
      catch (Exception ex)
      {
        ex.Should().BeOfType<NotFoundException>();
        ex.Message.Should().Be(exceptionMessage);
      }
    }

    [Fact]
    public async Task GetQuestions_ReturnQuesitons()
    {
      var questions = new List<ViewQuestionDto> { _viewQuestionDto };
      A.CallTo(() => _questionService.GetAllComplete())
        .Returns(Task.FromResult<IEnumerable<ViewQuestionDto>>([_viewQuestionDto]));

      var result = await _questionController.GetAllFull();

      result.Should().BeOfType<OkObjectResult>();
      var okResult = result as OkObjectResult;

      okResult?.StatusCode.Should().Be(StatusCodes.Status200OK);

      okResult?.Value.Should().BeEquivalentTo(questions);
    }

    /*[Fact]
    public async Task PatchUser_ReturnUser()
    {
        var newUserData = new UpdateQuestionDTO();
        /*var authUserId = "1";
        var httpContext = new DefaultHttpContext();
        var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
        new Claim("UserId", authUserId)
        }));
        httpContext.User = userClaims;
        _userController.ControllerContext.HttpContext = httpContext;


        A.CallTo(() => _questionService.PatchQuestion(1, newUserData))
            .Returns(Task.FromResult<BaseQuestion?>(_question));

        var result = await _questionController.PatchQuestion(1, newUserData);

        result.Result.Should().BeOfType<OkObjectResult>();
        var createdResult = result.Result as OkObjectResult;

        createdResult?.StatusCode.Should().Be(StatusCodes.Status200OK);

        createdResult?.Value.Should().BeSameAs(_question);
    }

    [Fact]
    public async Task DeleteQuestion_ReturnNoContent()
    {
        var authUserId = 1;

        A.CallTo(() => _questionService.DeleteQuestion(1))
            .Returns(true);

        var result = await _questionController.DeleteQuestion(authUserId);


        var createdResult = result as NoContentResult;

        createdResult?.StatusCode.Should().Be(StatusCodes.Status204NoContent);
    }

    [Fact]
    public async Task DeleteQuestion_QuestionNotFound_ReturnsNotFound()
    {
        // Arrange
        A.CallTo(() => _questionService.DeleteQuestion(1))
            .Throws(new WarningException());
        // Act
        var result = await _questionController.DeleteQuestion(1);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var notFoundResult = (NotFoundObjectResult)result;

        notFoundResult.StatusCode.Should().Be(404);

        notFoundResult.Value.Should().Be("Question id: 1 not found");
    }

    [Fact]
    public async Task DeleteQuestion_ExceptionOccurred_ReturnsInternalServerError()
    {
        A.CallTo(() => _questionService.DeleteQuestion(1))
            .Throws(new Exception("Test exception"));

        try
        {
            var result = await _questionController.DeleteQuestion(1);

        }
        catch (Exception result)
        {
            result.Should().BeOfType<Exception>();
            result.Message.Should()
                .Be("Error to delete Question");
        }

    }*/
  }
}
