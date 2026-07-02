using Biblioteca.Api.DTOs.Emprestimo;
using Biblioteca.Api.Mappers;
using Biblioteca.Api.Models;
using Biblioteca.Api.Repositories;
using Biblioteca.Api.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace Biblioteca.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmprestimosController : Controller
    {
        private readonly EmprestimoRepository _repository;
        private readonly UsuarioRepository _usuarioRepository;

        public EmprestimosController(EmprestimoRepository repository, UsuarioRepository usuarioRepository)
        {
            _repository = repository;
            _usuarioRepository = usuarioRepository;
        }

        //BUSCAR TODOS EMPRESTIMOS DE UM USUARIO
        [Authorize]
        [HttpGet("usuario/{id}")]
        public IActionResult GetByUser(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Id deve ser maior que zero"
                    });
                }

                string? usuarioLogado = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (!User.IsInRole("Administrador") && usuarioLogado != id.ToString())
                {
                    return StatusCode(StatusCodes.Status403Forbidden,
                        new ApiResponse<object>
                        {
                            Success = false,
                            Message = "Você não possui permissão para acessar os empréstimos deste usuário."
                        });
                }

                if (_usuarioRepository.Get(id) == null)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Usuário não encontrado"
                    });

                }


                List<Emprestimo> emprestimos = _repository.GetByUser(id);
                
                if(emprestimos.Count == 0)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Success = true,
                        Message = "Nenhum empréstimo encontrado"
                    });
                }

                List<EmprestimoResponse> response = [];

                foreach (Emprestimo emprestimo in emprestimos)
                {
                    response.Add(EmprestimoMapper.ToResponse(emprestimo));
                }

                return Ok(new ApiResponse<List<EmprestimoResponse>>
                {
                    Success = true,
                    Message = "Empréstimos encontrados",
                    Data = response
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "Erro interno no servidor",
                    Errors = new Dictionary<string, string>
                    {
                        { "server", ex.Message }
                    }
                });
            }
        }
    }
}
