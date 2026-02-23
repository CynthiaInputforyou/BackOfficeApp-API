using BackOfficeApp.Data;
using BackOfficeApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BackOfficeApp.Services
{
    public class UserService 
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        //Método crear usuario
        public async Task<User> CreateUser (User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        //Método editar usuario
        public async Task<User?> UpdateUser(int id, string name, string email)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return null;

            user.Name = name;
            user.Email = email;

            await _context.SaveChangesAsync();
            return user;
        }

        //Método paginar/listar con paginación
        public async Task<List<User>> GetPaged (int page, int pageSize)
        {
            return await _context.Users
            .OrderBy(u => u.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        }

    }
}
