using AutoMapper;
using FilmesAPI.Data.Dtos;
using FilmesAPI.Models;
using Microsoft.VisualBasic;

namespace FilmesAPI.Profiles;

public class FilmeProfile : Profile
{
    public FilmeProfile()
    {
        //Conversão de classe CreateFilmeDto para classe Filme:
        CreateMap<CreateFilmeDto, Filme>();
        //Conversão de classe UpdateFilmeDto para classe Filme:
        CreateMap<UpdateFilmeDto, Filme>();
        //Conversão de classe Filme para classe UpdateFilmeDto:
        CreateMap<Filme, UpdateFilmeDto>();
        //Conversão de classe Filme para classe ReadFilmeDto:
        CreateMap<Filme, ReadFilmeDto>().ForMember(filmeDto => filmeDto.Sessoes, opt => opt.MapFrom(sessao => sessao.Sessoes));
    }
}
