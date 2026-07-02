namespace Biblioteca.Api.Models
{
    public class Emprestimo
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public DateTime DataEmprestimo { get; set; }
        public DateTime? DataDevolucao { get; set; }
        public List<ItemEmprestimo> Itens { get; set; } = new();
    }
}
