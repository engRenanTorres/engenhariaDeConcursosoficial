using Apllication.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Apllication.Core;

public class MappingProfile : Profile
{
  public MappingProfile()
  {
    CreateMap<CreateQuestionDTO, MultipleChoicesQuestion>();
    CreateMap<CreateQuestionDTO, BooleanQuestion>();
  }
};
