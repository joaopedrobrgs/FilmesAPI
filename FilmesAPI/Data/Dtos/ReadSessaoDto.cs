using System.ComponentModel.DataAnnotations;

namespace FilmesAPI.Data.Dtos;

public class ReadSessaoDto
{
    public string Horario { get; set; }
    public string Idioma { get; set; }
    public int FilmeId { get; set; }
    public int CinemaId { get; set; }
}
