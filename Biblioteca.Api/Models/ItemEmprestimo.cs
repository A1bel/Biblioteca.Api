namespace Biblioteca.Api.Models
{
    public class ItemEmprestimo
    {
        public int Id { get; set; }
        public int IdEmprestimo { get; set; }
        public int IdLivro { get; set; }

        public Livro Livro { get; set; } = null!;
    }
}
