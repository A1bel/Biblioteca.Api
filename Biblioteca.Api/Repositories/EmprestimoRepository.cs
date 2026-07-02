using Biblioteca.Api.Database;
using Biblioteca.Api.Models;
using Microsoft.Data.SqlClient;

namespace Biblioteca.Api.Repositories
{
    public class EmprestimoRepository
    {
        private readonly DatabaseAccess _dbaccess;

        public EmprestimoRepository(DatabaseAccess dbaccess)
        {
            _dbaccess = dbaccess;
        }

        public List<Emprestimo> GetByUser(int idUsuario)
        {
            Dictionary<int, Emprestimo> emprestimos = new();

            using SqlConnection conection = _dbaccess.OpenConnection();
            using SqlCommand command = new SqlCommand();
            command.Connection = conection;

            command.CommandText = @"
                SELECT
                    e.id,
                    e.id_usuario,
                    e.data_emprestimo,
                    e.data_devolucao,

                    ie.id AS id_item,
                    ie.id_livro,

                    l.id,
                    l.categoria,
                    l.titulo,
                    l.quantidade_total,
                    l.quantidade_disponivel,
                    l.publicacao

                FROM emp.emprestimo e
                JOIN emp.item_emprestimo ie ON ie.id_emprestimo = e.id
                JOIN cad.livro l ON l.id = ie.id_livro

                WHERE e.id_usuario = @id_usuario
                ORDER BY e.id;
            ";

            command.Parameters.AddWithValue("@id_usuario", idUsuario);
            using SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                int idEmprestimo = Convert.ToInt32(reader["id"]);

                if (!emprestimos.TryGetValue(idEmprestimo, out Emprestimo? emprestimo))
                {
                    emprestimo = new Emprestimo
                    {
                        Id = idEmprestimo,
                        IdUsuario = Convert.ToInt32(reader["id_usuario"]),
                        DataEmprestimo = Convert.ToDateTime(reader["data_emprestimo"]),
                        DataDevolucao = reader["data_devolucao"] == DBNull.Value
                            ? null
                            : Convert.ToDateTime(reader["data_devolucao"])
                    };

                    emprestimos.Add(idEmprestimo, emprestimo);
                }

                ItemEmprestimo item = new()
                {
                    Id = Convert.ToInt32(reader["id_item"]),
                    IdEmprestimo = idEmprestimo,
                    IdLivro = Convert.ToInt32(reader["id_livro"]),

                    Livro = new Livro
                    {
                        Id = Convert.ToInt32(reader["id_livro"]),
                        Categoria = reader["categoria"].ToString()!,
                        Titulo = reader["titulo"].ToString()!,
                        QuantidadeTotal = Convert.ToInt32(reader["quantidade_total"]),
                        QuantidadeDisponivel = Convert.ToInt32(reader["quantidade_disponivel"]),
                        Publicacao = DateOnly.FromDateTime(Convert.ToDateTime(reader["publicacao"]))
                    }
                };

                emprestimo.Itens.Add(item);
            }

            return emprestimos.Values.ToList();
        }
    }
}
