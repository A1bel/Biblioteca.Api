using Biblioteca.Api.Database;
using Biblioteca.Api.Models;
using Microsoft.Data.SqlClient;

namespace Biblioteca.Api.Repositories
{
    public class UsuarioRepository
    {
        private readonly DatabaseAccess _dbaccess;

        public UsuarioRepository(DatabaseAccess dbaccess)
        {
            _dbaccess = dbaccess;
        }

        public List<Usuario> GetAll()
        {
            List<Usuario> usuarios = new List<Usuario>();

            using SqlConnection conection = _dbaccess.OpenConnection();
            using SqlCommand command = new SqlCommand();
            command.Connection = conection;

            command.CommandText = @"
                SELECT 
                    u.id,
                    u.id_perfil,
                    p.nome AS perfil,
                    u.nome,
                    u.cpf,
                    u.email
                FROM cad.usuario u
                JOIN cad.perfil p
                    ON u.id_perfil = p.id
            ";

            using SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Usuario usuario = new Usuario();
                usuario.Id = Convert.ToInt32(reader["id"]);
                usuario.IdPerfil = Convert.ToInt32(reader["id"]);
                usuario.Perfil = reader["perfil"].ToString();
                usuario.Nome = reader["nome"].ToString();
                usuario.Cpf = reader["cpf"].ToString();
                usuario.Email = reader["email"].ToString();
                usuarios.Add(usuario);
            }

            return usuarios;
        }

        public Usuario Get(int id)
        {
            using SqlConnection connection = _dbaccess.OpenConnection();
            using SqlCommand command = new SqlCommand();
            command.Connection = connection;

            command.CommandText = @"
                SELECT 
                    u.id,
                    u.id_perfil,
                    p.nome AS perfil,
                    u.nome,
                    u.cpf,
                    u.email
                FROM cad.usuario u
                JOIN cad.perfil p
                    ON u.id_perfil = p.id
                WHERE u.id = @id;
            ";

            command.Parameters.AddWithValue("@id", id);
            using SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                Usuario usuario = new Usuario
                {
                    Id = Convert.ToInt32(reader["id"]),
                    IdPerfil = Convert.ToInt32(reader["id"]),
                    Perfil = reader["perfil"].ToString(),
                    Nome = reader["nome"].ToString(),
                    Cpf = reader["cpf"].ToString(),
                    Email = reader["email"].ToString()
                };

                return usuario;
            }

            return null;
        }
    }
}
