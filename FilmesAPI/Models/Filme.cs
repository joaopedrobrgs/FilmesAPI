using System.ComponentModel.DataAnnotations;

namespace FilmesAPI.Models;

public class Filme
{
    //Chave primária:
    [Key]
    [Required]
    public int Id { get; set; }

    //Atributos comuns:
    [Required]
    public string Titulo { get; set; }
    [Required]
    [MaxLength(50)]
    public string Genero { get; set; }
    [Required]
    [Range(70, 600)]
    public int Duracao { get; set; }

    //Propriedades com as quais se relaciona:
    //Relação 1 : n
    public virtual ICollection<Sessao> Sessoes { get; set; }
}
