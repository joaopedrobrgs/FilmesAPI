using System.ComponentModel.DataAnnotations;

namespace FilmesAPI.Data.Dtos;

public class UpdateCinemaDto
{
    [Required(ErrorMessage = "O nome do cinema é obrigatório")]
    public string Nome { get; set; }
}
