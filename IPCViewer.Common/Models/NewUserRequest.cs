using System;
using System.Collections.Generic;
using System.Text;

namespace IPCViewer.Common.Models
{
    using System.ComponentModel.DataAnnotations;

    public class NewUserRequest
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public City City { get; set; }

        [Required]
        public int CityId { get; set; }
    }

}
