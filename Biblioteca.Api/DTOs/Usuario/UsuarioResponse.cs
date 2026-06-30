namespace Biblioteca.Api.DTOs.Usuario
{
    public class UsuarioResponse
    {
        public int Id { get; set; }
        public int IdPerfil { get; set; }
        public string Perfil { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
    }
}
