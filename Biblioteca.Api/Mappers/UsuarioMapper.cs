using Biblioteca.Api.DTOs.Usuario;
using Biblioteca.Api.Models;

namespace Biblioteca.Api.Mappers
{
    public static class UsuarioMapper
    {
        public static UsuarioResponse ToResponse(Usuario usuario)
        {
            return new UsuarioResponse
            {
                Id = usuario.Id,
                IdPerfil = usuario.IdPerfil,
                Perfil = usuario.Perfil,
                Nome = usuario.Nome,
                Cpf = usuario.Cpf,
                Email = usuario.Email
            };
        }

        public static List<UsuarioResponse> ToResponseList(List<Usuario> usuarios)
        {
            return usuarios
                .Select(ToResponse)
                .ToList();
        }
    }
}
