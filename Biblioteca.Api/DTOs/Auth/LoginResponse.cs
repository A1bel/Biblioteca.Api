using Biblioteca.Api.DTOs.Usuario;

namespace Biblioteca.Api.DTOs.Auth
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public UsuarioResponse Usuario { get; set; }
    }
}
