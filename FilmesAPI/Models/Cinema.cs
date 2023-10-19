using System.ComponentModel.DataAnnotations;

namespace FilmesAPI.Models
{
    public class Cinema
    {
        //Chave primária:
        [Key]
        [Required]
        public int Id { get; set; } 
        
        [Required]
        public string Nome { get; set; }

        //Chaves estrangeiras:
        [Required]
        public int EnderecoId { get; set; }

        //Propriedades com as quais se relaciona:
        //Relação 1 : 1
        public virtual Endereco Endereco { get; set; }
        //Relação 1 : n
        public virtual ICollection<Sessao> Sessoes { get; set; }
    }
}
