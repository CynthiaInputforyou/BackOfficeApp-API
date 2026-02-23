using BackOfficeApp.Dtos;
using BackOfficeApp.DTOs;
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
        public async Task<IActionResult> Post(CreateUserDto dto)
        {
            // Convertir DTO → Entidad
            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email
            };

            var created = await _service.CreateUser(user);

            // Convertir Entidad → DTO de salida
            var result = new UserDto
            {
                Id = created.Id,
                Name = created.Name,
                Email = created.Email
            };

            return Ok(result);
        }

        //Editar
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, UpdateUserDto dto)
        {
            // Llamamos al service pasando SOLO los datos necesarios
            // (no pasamos la entidad completa)
            var updated = await _service.UpdateUser(id, dto.Name, dto.Email);

            // Si no existe el usuario con ese id
            if (updated == null)
                return NotFound(); // 404

            // Convertimos la entidad a DTO de salida
            // Esto evita devolver el modelo interno directamente
            var result = new UserDto
            {
                Id = updated.Id,
                Name = updated.Name,
                Email = updated.Email
            };

            // Devolvemos 200 OK con el usuario actualizado
            return Ok(result);
        }

        //Listar por paginación
        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int pageSize = 10)
        {
            // Pedimos al service la lista paginada de entidades
            var users = await _service.GetPaged(page, pageSize);

            // Convertimos cada entidad a UserDto
            // Select transforma cada elemento de la lista
            var result = users.Select(u => new UserDto
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email
            }).ToList();

            // Devolvemos la lista convertida
            return Ok(result);
        }
    }
 }
