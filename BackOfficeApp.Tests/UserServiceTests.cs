using Xunit;
using BackOfficeApp.Services;
using BackOfficeApp.Data;
using BackOfficeApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace BackOfficeApp.Tests
{
    public class UserServiceTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

            return new AppDbContext(options);
        }

        //Test para POST
        [Fact]
        public async Task CreateUser_GuardaUnUsuario()
        {
            //1.Preparar
            var context = GetDbContext(); //Creo contexto
            var service = new UserService(context); //Creo service

            var user = new User { Name = "Natalia", Email = "natalia@test.com" }; //Creo datos

            // 2) Ejecutar
            await service.CreateUser(user);

            // 3) Comprobar
            Assert.Equal(1, context.Users.Count());
        }

        [Fact]
        public async Task UpdateUser_ModificaNombreYEmail()
        {
            //1.Preparar
            var context = GetDbContext();
            var service = new UserService(context);
            var user = new User
            {
                Name = "Antiguo",
                Email = "antiguo@test.com"
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();

            //2.Ejecutar
            await service.UpdateUser(user.Id, "Nuevo", "nuevo@test.com");

            //3.Comprobar
            var updatedUser = await context.Users.FindAsync(user.Id);

            Assert.Equal("Nuevo", updatedUser.Name);
            Assert.Equal("nuevo@test.com", updatedUser.Email);
        }

        //Null comprobacion
        [Fact]
        public async Task UpdateUser_SiNoExiste_DevuelveNull()
        {
            //1️.Preparar
            var context = GetDbContext();
            var service = new UserService(context);

            //2️.Ejecutar
            var result = await service.UpdateUser(999, "Nuevo", "nuevo@test.com");

            //3️.Comprobar
            Assert.Null(result);
        }

        //Paginación and GET
        [Fact]
        public async Task GetPaged_DevuelveCantidadCorrecta()
        {
            // 1️. Preparar
            var context = GetDbContext();
            var service = new UserService(context);

            for (int i = 1; i <= 10; i++)
            {
                context.Users.Add(new User
                {
                    Name = $"User{i}",
                    Email = $"user{i}@test.com"
                });
            }

            await context.SaveChangesAsync();

            // 2️.Ejecutar
            var result = await service.GetPaged(1, 10);

            // 3️.Comprobar
            Assert.Equal(10, result.Count);
        }
    }
}
