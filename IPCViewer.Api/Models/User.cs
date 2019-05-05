namespace IPCViewer.Api.Models
{
    using Microsoft.AspNetCore.Identity;
    using System.ComponentModel.DataAnnotations.Schema;

    /**
     * Clase Usuario que hereda de IdentityUsers (Usuarios de asp net core)
     * Le anado unas nuevas propiedades
     */
    public class User : IdentityUser
    {
        public string FirstName { get; set; }

        public override bool EmailConfirmed { get => base.EmailConfirmed; set => base.EmailConfirmed = value; }

        [NotMapped]
        public bool IsAdmin { get; set; }

        public City City { get; set; }

        public int CityId { get; set; }
    }
}

