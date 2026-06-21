using Biblioteca.Api.Models;
using Biblioteca.Api.Repositories;
using Biblioteca.Api.Responses;
using Biblioteca.Api.Validators;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LivrosController : Controller
    {

        private readonly LivroRepository _repository;

        public LivrosController(LivroRepository livroRepository)
        {
            _repository = livroRepository;
        }

        //BUSCAR TODOS LIVROS
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_repository.GetAll());
        }

        //BUSCAR UM LIVRO
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Id inválido",
                        Errors = new Dictionary<string, string>
                        {
                            { "id", "O id deve ser maior que zero" }
                        }
                    });
                }

                var livro = _repository.Get(id);

                if (livro == null)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Livro não encontrado",
                        Errors = new Dictionary<string, string>
                        {
                            { "id", "Nenhum livro encontrado com esse id" }
                        }
                    });
                }

                return Ok(new ApiResponse<Livro>
                {
                    Success = true,
                    Message = "Livro encontrado",
                    Data = livro
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

        //CADASTRAR NOVO LIVRO
        [HttpPost]
        public IActionResult Add(Livro livro)
        {
            try
            {

                LivroValidator validator = new();
                var errors = validator.Validate(livro);

                if (errors.Count > 0)
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Dados inválidos",
                        Errors = errors
                    });
                }

                _repository.Add(livro);
                return Ok(new ApiResponse<Livro>
                {
                    Success = true,
                    Message = "Livro cadastrado com sucesso",
                    Data = livro
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

        
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Id inválido"
                    });
                }

                bool deleted = _repository.Delete(id);

                if (!deleted)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Livro não encontrado"
                    });
                }

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Livro deletado com sucesso"
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
        //[HttpPut("{id}")]
    }
}
