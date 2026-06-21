using Biblioteca.Api.Database;
using Biblioteca.Api.Models;
using Microsoft.Data.SqlClient;

namespace Biblioteca.Api.Repositories
{
    public class LivroRepository
    {

        private readonly DatabaseAccess _dbaccess;

        public LivroRepository(DatabaseAccess dbaccess)
        {
            _dbaccess = dbaccess;
        }

        public List<Livro> GetAll()
        {
            List<Livro> livros = new List<Livro>();

            using SqlConnection connection = _dbaccess.OpenConnection();
            using SqlCommand command = new SqlCommand();
            command.Connection = connection;

            command.CommandText = @"SELECT id,
                                        categoria,
                                        titulo,
                                        quantidade_total,
                                        quantidade_disponivel,
                                        publicacao
                                        FROM cad.livro
                                        ORDER BY titulo";

            using SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Livro livro = new Livro();
                livro.Id = Convert.ToInt32(reader["id"]);
                livro.Categoria = reader["categoria"].ToString();
                livro.Titulo = reader["titulo"].ToString();
                livro.QuantidadeTotal = Convert.ToInt32(reader["quantidade_total"]);
                livro.QuantidadeDisponivel = Convert.ToInt32(reader["quantidade_disponivel"]);
                livro.Publicacao = DateOnly.FromDateTime(Convert.ToDateTime(reader["publicacao"]));
                livros.Add(livro);

            }

            return livros;
        }

        public Livro Get(int id)
        {

            using SqlConnection connection = _dbaccess.OpenConnection();
            using SqlCommand command = new SqlCommand();
            command.Connection = connection;

            command.CommandText = @"
                SELECT id,
                    categoria,
                    titulo,
                    quantidade_total,
                    quantidade_disponivel,
                    publicacao
                FROM cad.livro
                WHERE id = @id
            ";

            command.Parameters.AddWithValue("@id", id);
            using SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                Livro livro = new Livro
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Categoria = reader["categoria"].ToString(),
                    Titulo = reader["titulo"].ToString(),
                    QuantidadeTotal = Convert.ToInt32(reader["quantidade_total"]),
                    QuantidadeDisponivel = Convert.ToInt32(reader["quantidade_disponivel"]),
                    Publicacao = DateOnly.FromDateTime(Convert.ToDateTime(reader["publicacao"]))
                };

                return livro;
            }

            return null;
        }

        public void Add(Livro livro)
        {

            using SqlConnection connection = _dbaccess.OpenConnection();
            using SqlCommand command = new SqlCommand();
            command.Connection = connection;

            command.CommandText = @"
                INSERT INTO cad.livro(titulo, categoria, quantidade_total, quantidade_disponivel, publicacao)
                VALUES(@titulo, @categoria, @quantidade_total, @quantidade_disponivel, @publicacao)
            ";

            command.Parameters.AddWithValue("@titulo", livro.Titulo);
            command.Parameters.AddWithValue("@categoria", livro.Categoria);
            command.Parameters.AddWithValue("@quantidade_total", livro.QuantidadeTotal);
            command.Parameters.AddWithValue("@quantidade_disponivel", livro.QuantidadeTotal);
            command.Parameters.AddWithValue("@publicacao", livro.Publicacao);

            command.ExecuteNonQuery();

        }
    }
}
