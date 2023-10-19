using FilmesAPI.Models;
using FilmesAPI.Data;
using Microsoft.AspNetCore.Mvc;
using FilmesAPI.Data.Dtos;
using AutoMapper;
using Azure;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace FilmesAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class FilmeController : ControllerBase
{

    //Utilizando banco de dados:
    //Props:
    private FilmeContext _context;
    private IMapper _mapper;
    //Construtor:
    public FilmeController(FilmeContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    //Requisição para adicionar um filme à lista de filmes:

    //Documentação:
    /// <summary>
    /// Adiciona um filme ao banco de dados
    /// </summary>
    /// <param name="filmeDto">Objeto com os campos necessários para criação de um filme</param>
    /// <returns>IActionResult</returns>
    /// <response code="201">Caso inserção seja feita com sucesso</response>

    //Método:
    [HttpPost]
    //Informando qual tipo de resposta esse nosso método produz:
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult AdicionaFilme([FromBody] CreateFilmeDto filmeDto)
    {
        //Convertendo dados passados pelo usuário no corpo da requisição de CreateFilmeDto para Filme:
        Filme filme = _mapper.Map<Filme>(filmeDto);
        //Adicionando resultado dessa conversão ao banco de dados:
        _context.Filmes.Add(filme);
        //Salvando alteração no banco de dados:
        _context.SaveChanges();
        //Retornando resposta ao usuário (ação de criação de um objeto - código 201)
        return CreatedAtAction(nameof(RecuperaFilmePorId), new { id = filme.Id }, filme);
    }

    //Requisição que vai atualizar os dados de um filme:

    //Documentação:
    /// <summary>
    /// Atualiza os dados de um filme salvo no banco de dados através do seu ID
    /// </summary>
    /// <param name="id">ID do filme que vai ser atualizado, que vai ser passado via params</param>
    /// <param name="filmeDto">Objeto com os campos necessários para atualização do filme</param>
    /// <returns>IActionResult</returns>
    /// <response code="204">Caso atualização seja feita com sucesso</response>

    //Método:
    [HttpPut("{id}")]
    public IActionResult AtualizaFilme(int id, [FromBody] UpdateFilmeDto filmeDto)
    {
        //Encontrando filme pelo ID passado pelo usuário via params:
        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
        //Retornando nulo se nenhum filme for encontrado:
        if (filme == null) return NotFound();
        //Aplicando dados de objeto filmeDto (que pertence à classe UpdatedFilmeDto e veio da requisição do usuário) em objeto filme (que pertence à classe Filme e está "armazenado" no banco de dados):
        _mapper.Map(filmeDto, filme);
        //Salvando alteração no banco dados:
        _context.SaveChanges();
        //Retornando resposta ao usuário (ação de no content, normalmente utilizada para quando objeto é atualizado - código 204)
        return NoContent();
    }

    //Requisição que vai atualizar dados de um filme parcialmente:
    [HttpPatch("{id}")]
    public IActionResult AtualizaFilmeParcialmente(int id, JsonPatchDocument<UpdateFilmeDto> patch)
    {
        //Encontrando filme pelo ID passado pelo usuário via params:
        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
        //Retornando nulo se nenhum filme for encontrado:
        if (filme == null) return NotFound();
        //Convertendo objeto da classe Filme (que foi encontrado no banco através de seu id) para objeto da classe UpdatedFilmeDto e atribuindo resultado a uma variável:
        var filmeParaAtualizar = _mapper.Map<UpdateFilmeDto>(filme);
        //Aplicando mudança:
        patch.ApplyTo(filmeParaAtualizar, ModelState);
        //Validando mudança:
        if (!TryValidateModel(filmeParaAtualizar))
        {
            //Se não for válida, retornaremos um erro de validação (normalmente ocorre quando JSON enviado pelo usuário não condiz com que o modelo espera)
            return ValidationProblem(ModelState);
        }
        //Se for um ModelState válido, utilizar método "_mapper.Map()" para aplicar dados da variável filmeParaAtualizar (que acabou de ser validada - da classe UpdateFilmeDto) no objeto que foi encontrado no banco de dados (variável "filme" do tipo Filme):
        _mapper.Map(filmeParaAtualizar, filme);
        //Salvando alteração no banco dados:
        _context.SaveChanges();
        //Retornando resposta ao usuário (ação de no content, normalmente utilizada para quando objeto é atualizado ou removido - código 204)
        return NoContent();
    }

    //Requisição que vai deletar filme através do seu ID que será passado via params:
    [HttpDelete("{id}")]
    public IActionResult DeletaFilme(int id)
    {
        //Encontrando filmes através de seu ID:
        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
        //Retornando nulo se nenhum filme for encontrado:
        if (filme == null) return NotFound();
        //Apagando filme:
        _context.Filmes.Remove(filme);
        //Salvando alteração no banco de dados:
        _context.SaveChanges();
        //Retornando resposta ao usuário (ação de no content, normalmente utilizada para quando objeto é atualizado ou removido - código 204)
        return NoContent();
    }

    //////Requisição que vai retornar os filmes que foram adicionados à lista de filmes:
    [HttpGet]
    public IEnumerable<ReadFilmeDto> RecuperaFilmes([FromQuery] int skip = 0, [FromQuery] int take = 50, //[FromQuery] string? nomeCinema = null, 
        [FromQuery] int? idCinema = null)
    {
        //Convertendo Lista de Objetos da classe Filme (que foi encontrada no banco) para Lista de objetos da classe UpdatedFilmeDto, pois está classe nos retorna além dos atributos do filme também o horário da consulta pelo usuário, e em seguida retornando resultado ao usuário:
        if (idCinema == null)
        {
            return _mapper.Map<List<ReadFilmeDto>>(_context.Filmes.Skip(skip).Take(take).ToList());
        }
        //Pegando filmes de um cinema especifico:
        List<ReadFilmeDto> filmes = _mapper.Map<List<ReadFilmeDto>>(_context.Filmes.Skip(skip).Take(take).Where(filme => filme.Sessoes.Any(sessao => sessao.Cinema.Id == idCinema)).ToList());
        foreach (var filme in filmes)
        //Removendo sessões que vieram de forma errada junto com o filme:
        {
            List<ReadSessaoDto> sessoes = new List<ReadSessaoDto>();
            foreach (var sessao in filme.Sessoes)
            {
                if (sessao.CinemaId == idCinema)
                {
                    sessoes.Add(sessao);
                }
            }
            filme.Sessoes = sessoes;
        }
        return filmes;
    }

    ////Requisição que vai retornar filme especifico através de seu ID (passado via params):
    [HttpGet("{id}")] //GET + PARAMS
    public IActionResult RecuperaFilmePorId(int id)
    {
        //Encontrando filme pelo ID passado pelo usuário via params:
        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
        //Retornando nulo se nenhum filme for encontrado:
        if (filme == null) return NotFound();
        //Se filme for encontrado, primeiro convertemos ele para a classe ReadFilmeDto, que nos fornece além dos atributos do filme, a data da consulta pelo usuário:
        var filmeDto = _mapper.Map<ReadFilmeDto>(filme);
        //Retornando resposta ao usuário (ação OK, normalmente utilizada para quando caminho é encontrado - código 200)
        return Ok(filmeDto);
    }
}
