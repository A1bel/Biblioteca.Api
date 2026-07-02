namespace Biblioteca.Api.DTOs.Emprestimo
{
    public class EmprestimoResponse
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public DateTime DataEmprestimo { get; set; }
        public DateTime? DataDevolucao { get; set; }

        public List<LivroEmprestimoResponse> Livros { get; set; } = [];
    }
}
