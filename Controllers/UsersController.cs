using BackOfficeApp.Models;
using BackOfficeApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackOfficeApp.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly UserService _service;
        public UsersController(UserService service)
        {
            _service = service;
        }

        //HTTP DE LOS MÉTODOS
        //Crear
        [HttpPost]
        public async Task<IActionResult> Post(User user)
        {
            var created = await _service.CreateUser(user);
            return Ok(created);
        }

        //Editar
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, User user)
        {
            var updated = await _service.UpdateUser(id, user);
            if (updated == null)
                return NotFound();

            return Ok(updated);
        }

        //Listar por paginación
        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int pageSize = 10)
        {
            var users = await _service.GetPaged(page, pageSize);
            return Ok(users);
        }
    }
 }
