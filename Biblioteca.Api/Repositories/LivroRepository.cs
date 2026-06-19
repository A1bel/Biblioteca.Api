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

            try
            {
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
            }
            catch (Exception ex)
            {
                throw;
            }
            return livros;
        }
    }
}
