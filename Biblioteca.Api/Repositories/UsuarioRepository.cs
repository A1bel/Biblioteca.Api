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
                usuario.IdPerfil = Convert.ToInt32(reader["id_perfil"]);
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
                    IdPerfil = Convert.ToInt32(reader["id_perfil"]),
                    Perfil = reader["perfil"].ToString(),
                    Nome = reader["nome"].ToString(),
                    Cpf = reader["cpf"].ToString(),
                    Email = reader["email"].ToString()
                };

                return usuario;
            }

            return null;
        }


        public Usuario GetByEmail(string email)
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
                    u.email,
                    u.senha
                FROM cad.usuario u
                JOIN cad.perfil p ON u.id_perfil = p.id
                WHERE u.email = @email
            ";

            command.Parameters.AddWithValue("@email", email);
            using SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                Usuario usuario = new Usuario
                {
                    Id = Convert.ToInt32(reader["id"]),
                    IdPerfil = Convert.ToInt32(reader["id_perfil"]),
                    Perfil = reader["perfil"].ToString(),
                    Nome = reader["nome"].ToString(),
                    Cpf = reader["cpf"].ToString(),
                    Email = reader["email"].ToString(),
                    Senha = reader["senha"].ToString()
                };

                return usuario;
            }

            return null;
        }

        public int Add(Usuario usuario)
        {
            using SqlConnection connection = _dbaccess.OpenConnection();
            using SqlCommand command = new SqlCommand();
            command.Connection = connection;

            command.CommandText = @"
                INSERT INTO cad.usuario(id_perfil, nome, cpf, email, senha)
                OUTPUT INSERTED.id
                VALUES(@id_perfil, @nome, @cpf, @email, @senha)
            ";

            command.Parameters.AddWithValue("@id_perfil", usuario.IdPerfil);
            command.Parameters.AddWithValue("@nome", usuario.Nome);
            command.Parameters.AddWithValue("@cpf", usuario.Cpf);
            command.Parameters.AddWithValue("@email", usuario.Email);
            command.Parameters.AddWithValue("@senha", usuario.Senha);

            return (int)command.ExecuteScalar();
        }

        public bool ExistsCpf(string cpf, int? ignoredId = null)
        {
            using SqlConnection connection = _dbaccess.OpenConnection();
            using SqlCommand command = new SqlCommand();
            command.Connection = connection;

            command.CommandText = @"
                SELECT COUNT(1)
                FROM cad.usuario
                WHERE cpf = @cpf
            ";

            if (ignoredId != null)
            {
                command.CommandText += " AND id <> @id";
                command.Parameters.AddWithValue("@id", ignoredId);
            }

            command.Parameters.AddWithValue("@cpf", cpf);

            return (int)command.ExecuteScalar() > 0;
        }

        public bool ExistsEmail(string email, int? ignoredId = null)
        {
            using SqlConnection connection = _dbaccess.OpenConnection();
            using SqlCommand command = new SqlCommand();
            command.Connection = connection;

            command.CommandText = @"
                SELECT COUNT(1)
                FROM cad.usuario
                WHERE email = @email
            ";

            if (ignoredId != null)
            {
                command.CommandText += " AND id <> @id";
                command.Parameters.AddWithValue("@id", ignoredId);  
            }

            command.Parameters.AddWithValue("@email", email);

            return (int)command.ExecuteScalar() > 0;
        }

        public bool Delete(int id)
        {
            using SqlConnection connection = _dbaccess.OpenConnection();
            using SqlCommand command = new SqlCommand();
            command.Connection = connection;

            command.CommandText = @"
                DELETE FROM cad.usuario
                WHERE id = @id
            ";

            command.Parameters.AddWithValue("@id", id);
            int rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        }

        public bool Update(Usuario usuario)
        {
            using SqlConnection connection = _dbaccess.OpenConnection();
            using SqlCommand command = new SqlCommand();
            command.Connection = connection;

            command.CommandText = @"
                UPDATE cad.usuario
                SET
                    nome = @nome,
                    cpf = @cpf,
                    email = @email
                WHERE id = @id
            ";

            command.Parameters.AddWithValue("@id", usuario.Id);
            command.Parameters.AddWithValue("@nome", usuario.Nome);
            command.Parameters.AddWithValue("@cpf", usuario.Cpf);
            command.Parameters.AddWithValue("@email", usuario.Email);

            int rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        }

    }
}
