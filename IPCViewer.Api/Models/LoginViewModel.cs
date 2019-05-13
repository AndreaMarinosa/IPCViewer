namespace IPCViewer.Api.Models
{
    /**
     * ViewModel que conecta el login del app movi con la API
     * Se pasara un objeto LoginViewModel cuando se quiera logear en la aplicacion
     */

    public class LoginViewModel
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}