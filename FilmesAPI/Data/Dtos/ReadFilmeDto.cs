using FilmesAPI.Models;

namespace FilmesAPI.Data.Dtos;

public class ReadFilmeDto
{
    public int Id { get; set; }
    public string Titulo { get; set; }
    public string Genero { get; set; }
    public int Duracao { get; set; }
    //Retornando sessões em que esse filme será exibido:
    public ICollection<ReadSessaoDto> Sessoes { get; set; }
    //public DateTime HoraDaConsulta { get; set; } = DateTime.Now;
}
