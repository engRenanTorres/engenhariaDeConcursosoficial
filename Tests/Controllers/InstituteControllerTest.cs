using System.ComponentModel;
using System.Security.Claims;
using API.Controllers;
using Apllication.Core;
using Apllication.DTO;
using Apllication.DTOs;
using Apllication.DTOs.Institute;
using Apllication.Exceptions;
using Apllication.Services.Interfaces;
using Application.Core.PagedList;
using Application.DTOs;
using Application.DTOs.Institute;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DotnetAPITests.Controllers
{
  public class InstituteControllerTest
  {
    private readonly IInstituteService _instituteService;
    private readonly ILogger<InstituteController> _logger;
    private readonly InstituteController _instituteController;

    private readonly AppUser _appUser =
      new()
      {
        DisplayName = "TestUser",
        Email = "test@tes.com",
        UserName = "tester"
      };
    private readonly Institute _institute = new() { Id = Guid.NewGuid(), Name = "FGV", };
    private readonly ViewInstituteDto _viewInstituteDto;

    public InstituteControllerTest()
    {
      _viewInstituteDto = new()
      {
        Id = _institute.Id,
        Name = _institute.Name,
        CreatedAt = _institute.CreatedAt,
        About = _institute.About
      };
      _instituteService = A.Fake<IInstituteService>();
      _logger = A.Fake<ILogger<InstituteController>>();
      _instituteController = new InstituteController(_logger, _instituteService);
    }

    [Fact]
    public async Task CreateInstitute_ReturnQuetion()
    {
      var newInstitute = new CreateInstituteDto() { Name = "This is a quetion Test?", };
      var authUserId = "1";
      var httpContext = new DefaultHttpContext();
      var userClaims = new ClaimsPrincipal(
        new ClaimsIdentity(new Claim[] { new Claim("UserId", authUserId) })
      );
      httpContext.User = userClaims;
      _instituteController.ControllerContext.HttpContext = httpContext;

      A.CallTo(() => _instituteService.Create(newInstitute))
        .Returns(Task.FromResult<Institute?>(_institute));

      var result = await _instituteController.Create(newInstitute);

      result.Should().BeOfType<CreatedAtActionResult>();
      var createdResult = result as CreatedAtActionResult;

      createdResult?.StatusCode.Should().Be(StatusCodes.Status201Created);

      createdResult?.Value.Should().BeSameAs(_institute);
    }

    /*[Fact]
    public async Task CreateInstitute_InvalidUserId_ThrowsBadRequest()
    {
      var createInstituteDTO = new CreateInstituteDTO()
      {
        Body = "This is a quetion Test?",
        Answer = "A",
        Tip = "A",
        ConcursoId = _concurso.Id,
        SubjectId = _subject.Id,
        LevelId = _instituteLevel.Id
      };
      A.CallTo(() => _instituteService.Create(createInstituteDTO))
        .Throws(new BadRequestException("You must be loged to create."));
      var actionResult = await _instituteController.Create(createInstituteDTO);

      actionResult.Should().NotBeNull();
      actionResult.Should().BeOfType<NotFoundObjectResult>();
      var badRequestResult = actionResult as NotFoundObjectResult;

      badRequestResult?.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

      badRequestResult?.Value.Should().Be("You must be loged to create.");
    }

    [Fact]
    public async Task CreateInstitute_InvalidData_ThrowsBadRequest()
    {
      var authUserId = "1";
      var httpContext = new DefaultHttpContext();
      var userClaims = new ClaimsPrincipal(
        new ClaimsIdentity(new Claim[] { new Claim("UserId", authUserId) })
      );
      httpContext.User = userClaims;
      _instituteController.ControllerContext.HttpContext = httpContext;
      var createInstituteDTO = new CreateInstituteDTO()
      {
        Body = "This is a quetion Test?",
        Answer = "A",
        Tip = "A",
        ConcursoId = _concurso.Id,
        SubjectId = _subject.Id,
        LevelId = _instituteLevel.Id
      };

      A.CallTo(() => _instituteService.Create(createInstituteDTO))
        .Throws(new NotFoundException("Level not found."));

      var actionResult = await _instituteController.Create(createInstituteDTO);

      actionResult.Should().NotBeNull();
      actionResult.Should().BeOfType<NotFoundObjectResult>();
      var badRequestResult = actionResult as NotFoundObjectResult;

      badRequestResult?.StatusCode.Should().Be(StatusCodes.Status404NotFound);

      badRequestResult?.Value.Should().Be("Institute has Not been Created");
    }*/

    [Fact]
    public async Task GetInstitute_ReturnInstitute()
    {
      A.CallTo(() => _instituteService.GetById(_institute.Id)).Returns(Task.FromResult(_institute));

      var result = await _instituteController.GetFullById(_institute.Id);

      result.Should().BeOfType<OkObjectResult>();
      var createdResult = result as OkObjectResult;

      createdResult?.StatusCode.Should().Be(StatusCodes.Status200OK);

      createdResult?.Value.Should().BeEquivalentTo(_viewInstituteDto);
    }

    [Fact]
    public async Task GetInstitute_NotFoundInstitute_ThrowsNotFound()
    {
      var exceptionMessage = "Institute id: 1 not found";
      A.CallTo(() => _instituteService.GetById(_institute.Id))
        .Throws(new NotFoundException(exceptionMessage));

      try
      {
        var actionResult = await _instituteController.GetFullById(_institute.Id);
      }
      catch (Exception ex)
      {
        ex.Should().BeOfType<NotFoundException>();
        ex.Message.Should().Be(exceptionMessage);
      }
    }

    [Fact]
    public async Task DeleteInstitute_ReturnNoContent()
    {
      A.CallTo(() => _instituteService.Delete(_institute.Id));

      var result = await _instituteController.Delete(_institute.Id);

      var createdResult = result as NoContentResult;

      createdResult?.StatusCode.Should().Be(StatusCodes.Status204NoContent);
    }

    /*[Fact]
    public async Task DeleteInstitute_InstituteNotFound_ReturnsNotFound()
    {
      // Arrange
      A.CallTo(() => _instituteService.Delete(1))
        .Throws(new NotFoundException("Institute id: 1 not found"));
      // Act
      var result = await _instituteController.DeleteInstitute(1);

      // Assert
      result.Should().BeOfType<NotFoundObjectResult>();
      var notFoundResult = (NotFoundObjectResult)result;

      notFoundResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);

      notFoundResult.Value.Should().Be("Institute id: 1 not found");
    }

    [Fact]
    public async Task DeleteInstitute_ExceptionOccurred_ReturnsInternalServerError()
    {
      A.CallTo(() => _instituteService.Delete(1)).Throws(new Exception("Test exception"));

      try
      {
        var result = await _instituteController.DeleteInstitute(1);
      }
      catch (Exception result)
      {
        result.Should().BeOfType<Exception>();
        result.Message.Should().Be("Error to delete Institute");
      }
    }*/
  }
}
