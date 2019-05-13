using System.ComponentModel.DataAnnotations;

namespace IPCViewer.Common.Models
{
    public class UserEmailRequest
    {
        [Required]
        public string Email { get; set; }
    }
}