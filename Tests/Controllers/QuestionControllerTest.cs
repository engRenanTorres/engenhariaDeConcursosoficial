using System.ComponentModel;
using System.Security.Claims;
using API.Controllers;
using Apllication.Core;
using Apllication.DTOs;
using Apllication.Exceptions;
using Apllication.Services.Interfaces;
using Application.Core.PagedList;
using Application.DTOs;
using Domain.Entities;
using Domain.Entities.Questions;
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
    private readonly QuestionLevel _questionLevel =
      new()
      {
        Id = Guid.NewGuid(),
        Name = "Superior",
        CreatedAt = new DateTime(2020, 07, 02, 22, 59, 59),
      };
    private readonly Concurso _concurso =
      new()
      {
        Id = Guid.NewGuid(),
        Name = "Petrobras",
        CreatedAt = new DateTime(2020, 07, 02, 22, 59, 59),
        Institute = new() { Id = Guid.NewGuid(), Name = "Cebraspe" }
      };
    private readonly Subject _subject =
      new()
      {
        Id = Guid.NewGuid(),
        Name = "Estruturas",
        StudyArea = new() { Id = Guid.NewGuid(), Name = "EngCivil", }
      };
    private readonly ChoicesQuestion _question;

    private ViewQuestionDto _viewQuestionDto { get; set; }

    public QuestionControllerTest()
    {
      _question = new()
      {
        Id = 1,
        InsertedAt = new DateTime(2020, 07, 02, 22, 59, 59),
        LastUpdatedAt = new DateTime(2020, 07, 02, 22, 59, 59),
        Body = "Is this a quetion Test?",
        Answer = 'A',
        Tip = "A",
        InsertedBy = _appUser,
        Subject = _subject,
        Concurso = _concurso,
        QuestionLevel = _questionLevel
      };
      _viewQuestionDto = new ViewQuestionDto
      {
        Id = _question.Id,
        InsertedAt = _question.InsertedAt,
        LastUpdatedAt = _question.LastUpdatedAt,
        Body = _question.Body,
        Answer = _question.Answer,
        Tip = _question.Tip,
        InsertedBy = new LogedUserInfoDto
        {
          DisplayName = _question.InsertedBy.DisplayName,
          Username = _question.InsertedBy.UserName,
        },
        Subject = _subject.Name,
        Concurso = new()
        {
          Name = _question.Concurso.Name,
          InstituteName = _question.Concurso.Institute.Name,
          Year = _question.Concurso.Year
        },
        Level = _question.QuestionLevel.Name,
        StudyArea = _question.Subject.StudyArea.Name
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
        ConcursoId = _concurso.Id,
        SubjectId = _subject.Id,
        LevelId = _questionLevel.Id
      };
      var authUserId = "1";
      var httpContext = new DefaultHttpContext();
      var userClaims = new ClaimsPrincipal(
        new ClaimsIdentity(new Claim[] { new Claim("UserId", authUserId) })
      );
      httpContext.User = userClaims;
      _questionController.ControllerContext.HttpContext = httpContext;

      A.CallTo(() => _questionService.Create(newQuestion))
        .Returns(Task.FromResult<Question?>(_question));

      var result = await _questionController.Create(newQuestion);

      result.Should().BeOfType<CreatedAtActionResult>();
      var createdResult = result as CreatedAtActionResult;

      createdResult?.StatusCode.Should().Be(StatusCodes.Status201Created);

      createdResult?.Value.Should().BeSameAs(_question);
    }

    /*[Fact]
    public async Task CreateQuestion_InvalidUserId_ThrowsBadRequest()
    {
      var createQuestionDTO = new CreateQuestionDTO()
      {
        Body = "This is a quetion Test?",
        Answer = "A",
        Tip = "A",
        ConcursoId = _concurso.Id,
        SubjectId = _subject.Id,
        LevelId = _questionLevel.Id
      };
      A.CallTo(() => _questionService.Create(createQuestionDTO))
        .Throws(new BadRequestException("You must be loged to create."));
      var actionResult = await _questionController.Create(createQuestionDTO);

      actionResult.Should().NotBeNull();
      actionResult.Should().BeOfType<NotFoundObjectResult>();
      var badRequestResult = actionResult as NotFoundObjectResult;

      badRequestResult?.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

      badRequestResult?.Value.Should().Be("You must be loged to create.");
    }

    [Fact]
    public async Task CreateQuestion_InvalidData_ThrowsBadRequest()
    {
      var authUserId = "1";
      var httpContext = new DefaultHttpContext();
      var userClaims = new ClaimsPrincipal(
        new ClaimsIdentity(new Claim[] { new Claim("UserId", authUserId) })
      );
      httpContext.User = userClaims;
      _questionController.ControllerContext.HttpContext = httpContext;
      var createQuestionDTO = new CreateQuestionDTO()
      {
        Body = "This is a quetion Test?",
        Answer = "A",
        Tip = "A",
        ConcursoId = _concurso.Id,
        SubjectId = _subject.Id,
        LevelId = _questionLevel.Id
      };

      A.CallTo(() => _questionService.Create(createQuestionDTO))
        .Throws(new NotFoundException("Level not found."));

      var actionResult = await _questionController.Create(createQuestionDTO);

      actionResult.Should().NotBeNull();
      actionResult.Should().BeOfType<NotFoundObjectResult>();
      var badRequestResult = actionResult as NotFoundObjectResult;

      badRequestResult?.StatusCode.Should().Be(StatusCodes.Status404NotFound);

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
    public async Task GetQuestions_ReturnPagedListOfQuesitons()
    {
      var questions = new List<ViewQuestionDto> { _viewQuestionDto };
      var pagingParams = new PagingParams { PageNumber = 1, PageSize = 1 };
      var countQuestions = questions.Count;
      var pagedList = new PagedList<ViewQuestionDto>(
        questions,
        pagingParams.PageNumber,
        pagingParams.PageSize,
        countQuestions
      );
      A.CallTo(() => _questionService.GetAllComplete(pagingParams))
        .Returns(Task.FromResult<PagedList<ViewQuestionDto>>(pagedList));

      var result = await _questionController.GetAllFull(pagingParams);

      result.Should().BeOfType<OkObjectResult>();
      var okResult = result as OkObjectResult;

      okResult?.StatusCode.Should().Be(StatusCodes.Status200OK);

      okResult?.Value.Should().BeSameAs(pagedList);
    }

    [Fact]
    public async Task PatchUser_ReturnUser()
    {
      var newUserData = new UpdateQuestionDTO();
      var authUserId = "1";
      var httpContext = new DefaultHttpContext();
      var userClaims = new ClaimsPrincipal(
        new ClaimsIdentity(new Claim[] { new Claim("UserId", authUserId) })
      );
      httpContext.User = userClaims;
      _questionController.ControllerContext.HttpContext = httpContext;

      A.CallTo(() => _questionService.PatchQuestion(1, newUserData))
        .Returns(Task.FromResult<Question?>(_question));

      var result = await _questionController.PatchQuestion(1, newUserData);

      result.Should().BeOfType<OkObjectResult>();
      var createdResult = result as OkObjectResult;

      createdResult?.StatusCode.Should().Be(StatusCodes.Status200OK);

      createdResult?.Value.Should().BeSameAs(_question);
    }

    [Fact]
    public async Task DeleteQuestion_ReturnNoContent()
    {
      var authUserId = 1;

      A.CallTo(() => _questionService.Delete(1));

      var result = await _questionController.DeleteQuestion(authUserId);

      var createdResult = result as NoContentResult;

      createdResult?.StatusCode.Should().Be(StatusCodes.Status204NoContent);
    }

    /*[Fact]
    public async Task DeleteQuestion_QuestionNotFound_ReturnsNotFound()
    {
      // Arrange
      A.CallTo(() => _questionService.Delete(1))
        .Throws(new NotFoundException("Question id: 1 not found"));
      // Act
      var result = await _questionController.DeleteQuestion(1);

      // Assert
      result.Should().BeOfType<NotFoundObjectResult>();
      var notFoundResult = (NotFoundObjectResult)result;

      notFoundResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);

      notFoundResult.Value.Should().Be("Question id: 1 not found");
    }

    [Fact]
    public async Task DeleteQuestion_ExceptionOccurred_ReturnsInternalServerError()
    {
      A.CallTo(() => _questionService.Delete(1)).Throws(new Exception("Test exception"));

      try
      {
        var result = await _questionController.DeleteQuestion(1);
      }
      catch (Exception result)
      {
        result.Should().BeOfType<Exception>();
        result.Message.Should().Be("Error to delete Question");
      }
    }*/
  }
}
