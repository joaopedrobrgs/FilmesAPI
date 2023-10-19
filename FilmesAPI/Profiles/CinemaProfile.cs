using AutoMapper;
using FilmesAPI.Data.Dtos;
using FilmesAPI.Models;
using Microsoft.VisualBasic;

namespace FilmesAPI.Profiles;

public class CinemaProfile : Profile
{
    public CinemaProfile()
    {
        //Conversão de classe CreateCinemaDto para classe Cinema:
        CreateMap<CreateCinemaDto, Cinema>();
        //Conversão de classe UpdateCinemaDto para classe Cinema:
        CreateMap<UpdateCinemaDto, Cinema>();
        //Conversão de classe Cinema para classe UpdateCinemaDto:
        CreateMap<Cinema, UpdateCinemaDto>();
        //Conversão de classe Cinema para classe ReadCinemaDto:
        CreateMap<Cinema, ReadCinemaDto>().ForMember(cinemaDto => cinemaDto.Endereco, opt => opt.MapFrom(cinema => cinema.Endereco)).ForMember(filmeDto => filmeDto.Sessoes, opt => opt.MapFrom(sessao => sessao.Sessoes));
    }
}