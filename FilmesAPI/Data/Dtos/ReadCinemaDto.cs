namespace FilmesAPI.Data.Dtos;

public class ReadCinemaDto
{
    public int Id { get; set; }
    public string Nome { get; set; }
    //Retornando Endereço do cinema:
    public ReadEnderecoDto Endereco { get; set; }
    //Retornando todas as sessões desse cinema:
    public ICollection<ReadSessaoDto> Sessoes { get; set; }
    //public DateTime HoraDaConsulta { get; set; } = DateTime.Now;
}
