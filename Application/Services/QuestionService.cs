using System.ComponentModel;
using Apllication.DTOs;
using Apllication.Exceptions;
using Apllication.Repositories;
using Apllication.Services.Interfaces;
using Domain.Entities;
using Domain.Entities.Inharitance;
using Microsoft.Extensions.Logging;

namespace Apllication.Services;

public class QuestionService : IQuestionService
{
  private readonly IQuestionRepository _questionRepository;
  private readonly ILogger<IQuestionService> _logger;

  public QuestionService(IQuestionRepository questionRepository, ILogger<IQuestionService> logger)
  {
    _questionRepository = questionRepository;
    _logger = logger;
  }

  public async Task<IEnumerable<BaseQuestion?>> GetAllComplete()
  {
    throw new NotFoundException("Teste");
    var questions = await _questionRepository.GetAllComplete();
    return questions;
  }

  public async Task<BaseQuestion?> GetFullById(int id)
  {
    var questions = await _questionRepository.GetCompleteById(id);
    return questions;
  }

  public async Task<int> GetCount()
  {
    int numberOfQuestion = await _questionRepository.GetCount();
    return numberOfQuestion;
  }

  public async Task<int> GetLastId()
  {
    int numberOfQuestion = await _questionRepository.GetLastId();
    return numberOfQuestion;
  }

  public async Task<BaseQuestion> Create(CreateQuestionDTO questionDto)
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
      return question;
    }
    throw new Exception("Erro ao salvar a questão");
  }

  public async Task<bool> Delete(int id)
  {
    _logger.LogInformation("Delete Question has been called.");

    BaseQuestion question =
      await _questionRepository.GetById(id)
      ?? throw new WarningException("Question id: " + id + " not found");

    _questionRepository.Remove(question);
    return await _questionRepository.SaveChanges();
  }
}
