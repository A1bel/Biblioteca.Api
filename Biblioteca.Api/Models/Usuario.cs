namespace Biblioteca.Api.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public int IdPerfil { get; set; }
        public string Perfil { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }  

    }
}
