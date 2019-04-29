using IPCViewer.Api.Helpers;
using IPCViewer.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IPCViewer.Api.Services
{
    /**
     * Datos genéricos que se activa cuando borramos
     * la base de datos
     * 
     */
    public class SeedDb 
    {
        // DataContext privado que hemos recibido por parámetro
        private readonly DataContext context;
        private readonly IMailHelper mailHelper;
        private readonly IUserHelper userHelper;

        public SeedDb(DataContext context, IMailHelper mailHelper, IUserHelper userHelper)
        {
            this.context = context;
            this.userHelper = userHelper;
            this.mailHelper = mailHelper;

        }

        public async Task SeedAsync()
        {
            // Espera a que la base de datos esté creada antes de asignar datos
            await context.Database.EnsureCreatedAsync();

            // Comprueba si existen los roles creados
            await CheckRoles();

            if (!context.City.Any())
            {
                await AddCitiesAsync();
            }

            // Comprueba si el usuario administrador esta creado
            await CheckUserAsync("Juan@gmail.com", "Juan", "Perez", "Customer");
            var user = await CheckUserAsync("andreamarinosalopez@gmail.com", "Andrea", "Mariñosa", "Admin");

            // Si no hay ningun registro (false) creará 3 nuevas cámaras
            if (!context.Cameras.Any())
            {
                //AddCamera("A-2 Montañana", user, "http://infocar.dgt.es/etraffic/data/camaras/70.jpg", 41.655801, -0.878352);
                AddCamera("Puente Santiago", user, "http://webcam.abaco-digital.es/zuda/image2.jpg", 41.655801, -0.878352);
                AddCamera("Plaza del Pilar", user, "http://webcam.abaco-digital.es/pilar/image2.jpg", 41.657552, -0.881588);

                // Guardamos los cambios
                await context.SaveChangesAsync();
            }
        }

        private async Task CheckRoles()
        {
            await this.userHelper.CheckRoleAsync("Admin");
            await this.userHelper.CheckRoleAsync("Customer");

        }

        private async Task AddCitiesAsync()
        {
            context.City.Add(new City
            {
                Name = "Zaragoza"
            });

            await context.SaveChangesAsync();
        }

        private async Task<User> CheckUserAsync(string userName, string firstName, string lastName, string role)
        {
            // Busca si existe el email Administrador
            var user = await userHelper.GetUserByEmailAsync(userName);

            // Si no existe, lo crea
            if (user == null)
            {
                user = await AddUser(userName, firstName, lastName, role);

                var isInRole = await this.userHelper.IsUserInRoleAsync(user, role);
                if (!isInRole)
                {
                    await userHelper.AddUserToRoleAsync(user, role);
                }

            }
            return user;

        }

        private async Task<User> AddUser(string userName, string firstName, string lastName, string role)
        {
            var user = new User
            {
                FirstName = firstName,
                LastName = lastName,
                Email = userName,
                UserName = userName,
                City = context.City.FirstOrDefault(),
            };

            // Creamos en la base de datos
            var result = await this.userHelper.AddUserAsync(user, "123456");
            if (result != IdentityResult.Success)
            {
                throw new InvalidOperationException("Could not create the user in seeder");
            }

            await this.userHelper.AddUserToRoleAsync(user, role);
            var token = await this.userHelper.GenerateEmailConfirmationTokenAsync(user);
            await this.userHelper.ConfirmEmailAsync(user, token);
            return user;
        }


        private void AddCamera(string name, User user, string imgUrl, double latitude, double longitude) => context.Cameras.Add(new Camera
        {
            Name = name,
            Latitude = latitude,
            Longitude = longitude,
            CreatedDate = DateTime.Now,
            User = user,
            City = context.City.FirstOrDefault(),
            ImageUrl = imgUrl
        });
    
    }
}
