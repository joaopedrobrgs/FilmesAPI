using System.ComponentModel.DataAnnotations;

namespace FilmesAPI.Data.Dtos;

public class CreateSessaoDto
{
    [Required(ErrorMessage = "O horário da seção é obrigatório")]
    public string Horario { get; set; }
    [Required(ErrorMessage = "O idioma da seção é obrigatório")]
    public string Idioma { get; set; }
    public int FilmeId { get; set; }
    public int CinemaId { get; set; }
}
