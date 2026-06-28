using Biblioteca.Api.DTOs;
using Biblioteca.Api.Models;
using Biblioteca.Api.Repositories;
using Biblioteca.Api.Responses;
using Biblioteca.Api.Validators;
using Microsoft.AspNetCore.Mvc;
using BCrypt.Net;

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

        //CADASTRAR NOVO USUARIO
        [HttpPost]
        public IActionResult Add(UsuarioCreateRequest dto)
        {
            try
            {
                UsuarioValidator validator = new UsuarioValidator();
                var errors = validator.ValidateCreate(dto);

                if (!errors.ContainsKey("cpf") && _repository.ExistsCpf(dto.Cpf))
                {
                    errors.Add("cpf", "CPF já cadastrado");
                }

                if (!errors.ContainsKey("email") && _repository.ExistsEmail(dto.Email))
                {
                    errors.Add("email", "E-mail já cadastrado");
                }

                if (errors.Count > 0)
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Dados inválidos",
                        Errors = errors
                    });
                }

                Usuario usuario = new Usuario
                {
                    IdPerfil = 2,
                    Perfil = "Cliente",
                    Nome = dto.Nome,
                    Cpf = dto.Cpf,
                    Email = dto.Email,
                    Senha = BCrypt.Net.BCrypt.HashPassword(dto.Senha)
                };

                _repository.Add(usuario);
                return Ok(new ApiResponse<Usuario>
                {
                    Success = true,
                    Message = "Usuário cadastrado com sucesso",
                    Data = new Usuario
                    {
                        Id = usuario.Id,
                        IdPerfil= usuario.IdPerfil,
                        Perfil = usuario.Perfil,
                        Nome = usuario.Nome,
                        Cpf = usuario.Cpf,
                        Email = usuario.Email
                    }
                });
            }
            catch(Exception ex)
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
