using System.ComponentModel.DataAnnotations;

namespace FilmesAPI.Models;

public class Sessao
{
    //Atributos comuns:
    [Required]
    public string Horario { get; set; }
    [Required]
    public string Idioma { get; set; }

    //Chaves estrangeiras:
    public int? FilmeId { get; set; }
    public int? CinemaId { get; set; }

    //Propriedades com as quais se relaciona:
    //Relação n : 1
    public virtual Filme Filme { get; set; }
    //Relação n : 1
    public virtual Cinema Cinema { get; set; }

}
