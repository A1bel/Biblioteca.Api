using Biblioteca.Api.DTOs.Emprestimo;
using Biblioteca.Api.Models;

namespace Biblioteca.Api.Mappers
{
    public static class EmprestimoMapper
    {
        public static EmprestimoResponse ToResponse(Emprestimo emprestimo)
        {
            EmprestimoResponse response = new()
            {
                Id = emprestimo.Id,
                IdUsuario = emprestimo.IdUsuario,
                DataEmprestimo = emprestimo.DataEmprestimo,
                DataDevolucao = emprestimo.DataDevolucao
            };

            foreach (ItemEmprestimo item in emprestimo.Itens)
            {
                response.Livros.Add(new LivroEmprestimoResponse
                {
                    Id = item.Livro.Id,
                    Titulo = item.Livro.Titulo
                });
            }

            return response;
        }
    }
}
