using AutoMapper;
using FilmesAPI.Data;
using FilmesAPI.Data.Dtos;
using FilmesAPI.Models;
using FilmesAPI.Profiles;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace FilmesAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CinemaController : ControllerBase
{

    private FilmeContext _context;
    private IMapper _mapper;

    public CinemaController(FilmeContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet, Route("")]
    public IEnumerable<ReadCinemaDto> RecuperaCinemas([FromQuery] int skip = 0, [FromQuery] int take = 50, [FromQuery] int? enderecoId = null)
    {
        if (enderecoId == null)
        {
            return _mapper.Map<List<ReadCinemaDto>>(_context.Cinemas.Skip(skip).Take(take).ToList());
        }
        //Pegando cinema de um endereço especifico:
        return _mapper.Map<List<ReadCinemaDto>>(_context.Cinemas.FromSqlRaw($"SELECT Id, Nome, EnderecoId FROM Cinemas WHERE Cinemas.EnderecoId = {enderecoId}").Skip(skip).Take(take).ToList());
    }

    [HttpGet("{id}"), Route("")]
    public IActionResult RecuperaCinemaPorId(int id)
    {
        var cinema = _context.Cinemas.FirstOrDefault(cinema => cinema.Id == id);
        if (cinema == null) return NotFound();
        var cinemaDto = _mapper.Map<ReadCinemaDto>(cinema);
        return Ok(cinemaDto);
    }

    [HttpPost, Route("")]
    public IActionResult AdicionaCinema([FromBody] CreateCinemaDto cinemaDto)
    {
        Cinema cinema = _mapper.Map<Cinema>(cinemaDto);
        _context.Cinemas.Add(cinema);
        _context.SaveChanges();
        return CreatedAtAction(nameof(RecuperaCinemaPorId), new { Id = cinema.Id }, cinema);
    }

    [HttpPut("{id}"), Route("")]
    public IActionResult AtualizaCinema(int id, [FromBody] UpdateCinemaDto cinemaDto)
    {
        var cinema = _context.Cinemas.FirstOrDefault(cinema => id == cinema.Id);
        if (cinema == null) return NotFound();
        _mapper.Map(cinemaDto, cinema);
        _context.SaveChanges();
        return NoContent();
    }

    [HttpPatch("{id}"), Route("")]
    public IActionResult AtualizaCinemaParcialmente(int id, JsonPatchDocument<UpdateCinemaDto> patch)
    {
        var cinema = _context.Cinemas.FirstOrDefault(cinema => id == cinema.Id);
        if (cinema == null) return NotFound();
        var cinemaDto = _mapper.Map<UpdateCinemaDto>(cinema);
        patch.ApplyTo(cinemaDto, ModelState);
        if (!TryValidateModel(cinemaDto))
        {
            return ValidationProblem(ModelState);
        }
        _mapper.Map(cinemaDto, cinema);
        _context.SaveChanges();
        return NoContent();
    }

    [HttpDelete("{id}"), Route("")]
    public IActionResult DeletaCinema(int id)
    {
        var cinema = _context.Cinemas.FirstOrDefault(cinema => cinema.Id == id);
        if (cinema == null) return NotFound();
        _context.Cinemas.Remove(cinema);
        _context.SaveChanges();
        return NoContent();
    }


}
