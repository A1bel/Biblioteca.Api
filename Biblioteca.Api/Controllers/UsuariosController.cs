using Biblioteca.Api.Repositories;
using Biblioteca.Api.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : Controller
    {
        private readonly UsuarioRepository _repository;

        public UsuariosController(UsuarioRepository usuarioRepository)
        {
            _repository = usuarioRepository;
        }

        //BUSCAR TODOS USUARIOS
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_repository.GetAll());
        }

        //BUSCAR UM USUARIO
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                if(id <= 0)
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Id deve ser maior que zero"
                    });
                }

                var usuario = _repository.Get(id);

                if(usuario == null)
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Usuário não encontrado"
                    });
                }

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Usuário encontrado",
                    Data = usuario
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
