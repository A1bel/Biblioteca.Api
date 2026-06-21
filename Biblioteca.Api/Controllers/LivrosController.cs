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

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_repository.GetAll());
        }


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

        //[HttpGet("{id}")]
        //[HttpDelete("{id}")]
        //[HttpPut("{id}")]
    }
}
