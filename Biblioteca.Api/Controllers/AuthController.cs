using Biblioteca.Api.DTOs.Auth;
using Biblioteca.Api.Mappers;
using Biblioteca.Api.Models;
using Biblioteca.Api.Repositories;
using Biblioteca.Api.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Biblioteca.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly UsuarioRepository _repository;
        private readonly IConfiguration _configuration;

        public AuthController(UsuarioRepository repository, IConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;
        }

        [AllowAnonymous]
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

                bool senhaValida =  BCrypt.Net.BCrypt.Verify(dto.Senha, usuario.Senha);

                if (!senhaValida)
                {
                    return Unauthorized(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "E-mail ou senha inválidos"
                    });
                }

                string token = GenerateToken(usuario);

                LoginResponse response = new()
                {
                    Token = token,
                    Expiration = DateTime.UtcNow.AddMinutes(
                        Convert.ToDouble(_configuration["Jwt:ExpirationInMinutes"])),

                    Usuario = UsuarioMapper.ToResponse(usuario)
                };

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Login realizado com sucesso",
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

        private string GenerateToken(Usuario usuario)
        {
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Nome),
                new Claim(ClaimTypes.Role, usuario.Perfil)
            };

            var credentials = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(
                Convert.ToDouble(_configuration["Jwt:ExpirationInMinutes"])),
            signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
