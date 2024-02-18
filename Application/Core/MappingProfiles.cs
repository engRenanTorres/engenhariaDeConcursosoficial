using Apllication.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.Inharitance;

namespace Apllication.Core;

public class MappingProfile : Profile
{
  public MappingProfile()
  {
    CreateMap<CreateQuestionDTO, Question>();
  }
};
