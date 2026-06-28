namespace Biblioteca.Api.DTOs
{
    public class UsuarioCreateRequest
    {
        public int IdPerfil { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string ConfirmarSenha { get; set; }
    }
}
