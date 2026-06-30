using BCrypt.Net;
using Biblioteca.Api.DTOs;
using Biblioteca.Api.Mappers;
using Biblioteca.Api.Models;
using Biblioteca.Api.Repositories;
using Biblioteca.Api.Responses;
using Biblioteca.Api.Validators;
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
            List<Usuario> usuarios = _repository.GetAll();
            return Ok(new ApiResponse<List<UsuarioResponse>>
            {
                Success = true,
                Message = "Usuários encontrados",
                Data = UsuarioMapper.ToResponseList(usuarios)
            });
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
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Usuário não encontrado"
                    });
                }

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Usuário encontrado",
                    Data = UsuarioMapper.ToResponse(usuario)
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
                UsuarioValidator validator = new();
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

                usuario.Id = _repository.Add(usuario);
                return Ok(new ApiResponse<UsuarioResponse>
                {
                    Success = true,
                    Message = "Usuário cadastrado com sucesso",
                    Data = UsuarioMapper.ToResponse(usuario)
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

        //DELETAR UM LIVRO
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
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

                bool deleted = _repository.Delete(id);

                if (!deleted)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Usuário não encontrado"
                    });
                }

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Usuário deletado com sucesso"
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

        //EDITAR UM USUARIO
        [HttpPut("{id}")]
        public IActionResult Update(int id, UsuarioUpdateRequest dto)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "O id deve ser maior que zero"
                    });
                }

                UsuarioValidator validator = new();
                var errors = validator.ValidateUpdate(dto);

                if (!errors.ContainsKey("cpf") && _repository.ExistsCpf(dto.Cpf, id))
                {
                    errors.Add("cpf", "CPF já cadastrado");
                }

                if (!errors.ContainsKey("email") && _repository.ExistsEmail(dto.Email, id))
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

                Usuario usuarioAtual = _repository.Get(id);

                if(usuarioAtual == null)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Usuário não encontrado"
                    });
                }

                Usuario usuario = new Usuario
                {
                    Id = id,
                    Nome = dto.Nome,
                    Cpf = dto.Cpf,
                    Email = dto.Email
                };

                bool updated = _repository.Update(usuario);

                if (!updated)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Usuário não encontrado"
                    });
                }

                Usuario usuarioAtualizado = _repository.Get(id);

                return Ok(new ApiResponse<UsuarioResponse>
                {
                    Success = true,
                    Message = "Usuário atualizado com sucesso",
                    Data = UsuarioMapper.ToResponse(usuarioAtualizado)
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
