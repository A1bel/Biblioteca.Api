using Biblioteca.Api.Repositories;
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

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_repository.GetAll());
        }
    }
}
