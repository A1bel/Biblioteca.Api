using Biblioteca.Api.Models;

namespace Biblioteca.Api.Validators
{
    public class LivroValidator
    {
        public Dictionary<string, string> Validate(Livro livro)
        {
            Dictionary<string, string> errors = [];

            if (string.IsNullOrWhiteSpace(livro.Titulo))
                errors.Add("titulo", "Título obrigatório");

            if (string.IsNullOrWhiteSpace(livro.Categoria))
                errors.Add("categoria", "Categoria obrigatória");

            if (livro.QuantidadeTotal < 0)
                errors.Add("quantidade", "Quantidade não pode ser negativa");

            if (livro.Publicacao > DateOnly.FromDateTime(DateTime.Now))
                errors.Add("publicacao", "Data não pode ser no futuro");

            return errors;
        }
    }
}