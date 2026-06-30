using Biblioteca.Api.DTOs;
using Biblioteca.Api.Models;
using Biblioteca.Api.Repositories;
using Biblioteca.Api.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly UsuarioRepository _repository;

        public AuthController(UsuarioRepository repository)
        {
            _repository = repository;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequest dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.Email) ||
                    string.IsNullOrWhiteSpace(dto.Senha))
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "E-mail e senha são obrigatórios"
                    });
                }

                Usuario usuario = _repository.GetByEmail(dto.Email);

                if (usuario == null)
                {
                    return Unauthorized(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "E-mail ou senha inválidos"
                    });
                }

                bool senhaValida =
                    BCrypt.Net.BCrypt.Verify(dto.Senha, usuario.Senha);

                if (!senhaValida)
                {
                    return Unauthorized(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "E-mail ou senha inválidos"
                    });
                }

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Login realizado com sucesso"
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
