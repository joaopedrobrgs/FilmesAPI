using AutoMapper;
using Azure;
using FilmesAPI.Data;
using FilmesAPI.Data.Dtos;
using FilmesAPI.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FilmesAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class EnderecoController : ControllerBase
{
    private FilmeContext _context;
    private IMapper _mapper;

    public EnderecoController(FilmeContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet, Route("")]
    public IEnumerable<ReadEnderecoDto> RecuperaEnderecos([FromQuery] int skip = 0, [FromQuery] int take = 50, string? cinema = null)
    {
        if (cinema == null)
        {
            return _mapper.Map<List<ReadEnderecoDto>>(_context.Enderecos.Skip(skip).Take(take).ToList());
        }
        //Pegando endereço de um cinema especifico:
        return _mapper.Map<List<ReadEnderecoDto>>(_context.Enderecos.FromSqlRaw($"SELECT e.Id as EnderecoId, e.Logradouro, e.Numero , c.Id, c.Nome, c.EnderecoId as ForeignerKey FROM Enderecos e INNER JOIN Cinemas c ON e.Id = c.EnderecoId WHERE c.Nome = '{cinema}'").Skip(skip).Take(take).ToList());
    }

    [HttpGet("{id}"), Route("")]
    public IActionResult RecuperaEnderecoPorId(int id)
    {
        var endereco = _context.Enderecos.FirstOrDefault(endereco => endereco.Id == id);
        if (endereco != null)
        {
            ReadEnderecoDto enderecoDto = _mapper.Map<ReadEnderecoDto>(endereco);
            return Ok(enderecoDto);
        }
        return NotFound();
    }

    [HttpPost, Route("")]
    public IActionResult AdicionaEndereco([FromBody] CreateEnderecoDto enderecoDto)
    {
        Endereco endereco = _mapper.Map<Endereco>(enderecoDto);
        _context.Enderecos.Add(endereco);
        _context.SaveChanges();
        return CreatedAtAction(nameof(RecuperaEnderecoPorId), new { Id = endereco.Id }, endereco);
    }

    [HttpDelete("{id}"), Route("")]
    public IActionResult DeletaEndereco(int id)
    {
        var endereco = _context.Enderecos.FirstOrDefault(endereco => endereco.Id == id);
        if (endereco != null)
        {
            _context.Enderecos.Remove(endereco);
            _context.SaveChanges();
            return NoContent();
        }
        return NotFound();
    }

    [HttpPut("{id}"), Route("")]
    public IActionResult AtualizaEndereco(int id, [FromBody] UpdateEnderecoDto enderecoDto)
    {
        var endereco = _context.Enderecos.FirstOrDefault(endereco => endereco.Id == id);
        if (endereco != null)
        {
            _mapper.Map(enderecoDto, endereco);
            _context.SaveChanges();
            return NoContent();
        }
        return NotFound();
    }

    [HttpPatch("{id}"), Route("")]
    public IActionResult AtualizaEnderecoParcialmente(int id, JsonPatchDocument<UpdateEnderecoDto> patch)
    {
        var endereco = _context.Enderecos.FirstOrDefault(endereco => endereco.Id == id);
        if (endereco != null)
        {
            UpdateEnderecoDto enderecoDto = _mapper.Map<UpdateEnderecoDto>(endereco);
            patch.ApplyTo(enderecoDto, ModelState);
            if (!TryValidateModel(enderecoDto))
            {
                return ValidationProblem(ModelState);
            }
            _mapper.Map(enderecoDto, endereco);
            _context.SaveChanges();
            return NoContent();
        }
        return NotFound();
    }
}
