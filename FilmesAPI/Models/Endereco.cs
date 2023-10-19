using System.ComponentModel.DataAnnotations;

namespace FilmesAPI.Models;

public class Endereco
{
    //Chave primária:
    [Key]
    [Required]
    public int Id { get; set; }
    
    [Required]
    public string Logradouro { get; set; }
    [Required]
    public int Numero { get; set; }

    //Tabela com a qual se relaciona:
    public virtual Cinema Cinema { get; set; }
}
