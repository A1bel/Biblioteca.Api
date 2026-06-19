using Microsoft.AspNetCore.Http.HttpResults;
using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Api.Models
{
    public class Livro
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "A categoria é obrigatória.")]
        public string Categoria { get; set; }
        [Required(ErrorMessage = "O título é obrigatório.")]
        public string Titulo { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "A quantidade total não pode ser menor que zero.")]
        public int QuantidadeTotal { get; set; }
        public int QuantidadeDisponivel { get; set; }
        [Required(ErrorMessage = "A data de publicação é obrigatória.")]
        public DateOnly Publicacao { get; set; }
    }
}
