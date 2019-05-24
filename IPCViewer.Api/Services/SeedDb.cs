using IPCViewer.Api.Helpers;
using IPCViewer.Api.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;
using IPCViewer.Api.Data;

namespace IPCViewer.Api.Services
{
    /**
     * Clase que implementa datos genericos cuando la base
     * de datos esta vacia
     */

    public class SeedDb
    {
        #region Private Properties

        private readonly DataContext context;
        private readonly IUserHelper userHelper;

        #endregion Private Properties

        #region Constructor

        public SeedDb (DataContext context, IUserHelper userHelper)
        {
            this.context = context;
            this.userHelper = userHelper;
        }

        #endregion Constructor

        public async Task SeedAsync ()
        {
            // Espera y comprueba que la base de datos esté creada antes de asignar nuevos
            await context.Database.EnsureCreatedAsync();

            // Comprueba si existen los roles creados
            await CheckRoles();

            // Si la base de datos no contiene ciudades, las crea
            if ( !context.City.Any() )
            {
                await AddCitiesAsync();
            }

            // Comprueba si el usuario administrador esta creado
            //await CheckUserAsync("Juan@gmail.com", "Juan", "Customer");
            var user = await CheckUserAsync("andreamarinosalopez@gmail.com", "Andrea", "Admin");

            // Si no hay ningun registro (false) creará 3 nuevas cámaras
            if ( !context.Cameras.Any() )
            {
                //AddCamera("A-2 Montañana", user, "http://infocar.dgt.es/etraffic/data/camaras/70.jpg", 41.655801, -0.878352);
                AddCamera("Puente Santiago", user, "http://webcam.abaco-digital.es/zuda/image2.jpg", 41.655801, -0.878352);
                AddCamera("Plaza del Pilar", user, "http://webcam.abaco-digital.es/pilar/image2.jpg", 41.657552, -0.881588);

                // Guardamos los cambios
                await context.SaveChangesAsync();
            }
        }

        /**
         * Comprueba que existe los roles "admin" y "customer".
         * De no existir los crea
         */

        private async Task CheckRoles ()
        {
            await this.userHelper.CheckRoleAsync("Admin");
            await this.userHelper.CheckRoleAsync("Customer");
        }

        /**
         * Aniade la ciudad "Zaragoza" si no hay ciudades creadas
         */

        private async Task AddCitiesAsync ()
        {
            // aniade la ciudad
            context.City.Add(new City
            {
                Name = "Zaragoza"
            });

            // Guarda los cambios en bbdd
            await context.SaveChangesAsync();
        }

        /**
         * Comprueba que existe este usuario
         */

        private async Task<User> CheckUserAsync (string userName, string firstName, string role)
        {
            // Busca el usuario mediante su email
            var user = await userHelper.GetUserByEmailAsync(userName);

            // Si no existe, lo crea
            if ( user == null )
            {
                user = await AddUser(userName, firstName, role);

                /*
                 * Comprueba si el usuario pasado por parametro tiene rol
                 * Devuelve true/false
                 */
                var isInRole = await this.userHelper.IsUserInRoleAsync(user, role);
                // si no tiene role, se le asigna
                if ( !isInRole )
                {
                    await userHelper.AddUserToRoleAsync(user, role);
                }
            }
            return user;
        }

        /**
         * Metodo que agrega un nuevo usuario a la base de datos
         */

        private async Task<User> AddUser (string userName, string firstName, string role)
        {
            // El nombre de usuario y el email siempre serán los mismos
            var user = new User // Model.User
            {
                FirstName = firstName,
                Email = userName,
                UserName = userName,
                City = context.City.FirstOrDefault(),
            };

            // Creamos el usuario en la base de datos
            var result = await this.userHelper.AddUserAsync(user, "123456");
            if ( result != IdentityResult.Success )
            {
                throw new InvalidOperationException("Could not create the user in seeder");
            }

            // Aniadimos el rol al usuario y guardamos en bbdd
            await this.userHelper.AddUserToRoleAsync(user, role);
            // Generamos el email de confirmacion
            //var token = await this.userHelper.GenerateEmailConfirmationTokenAsync(user);
            //// Confirmamos el email
            //await this.userHelper.ConfirmEmailAsync(user, token);
            return user;
        }

        /**
         * Metodo que añade 2 nuevas camaras a la bbdd
         */

        private void AddCamera (string name, User user, string imgUrl, double latitude, double longitude) => context.Cameras.Add(new Camera
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