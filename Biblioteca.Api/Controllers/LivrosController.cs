using Biblioteca.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LivrosController : Controller
    {

        //public IActionResult Index()
        //{
        //    return View();
        //}

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
    }
}
