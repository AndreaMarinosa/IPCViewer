namespace IPCViewer.Forms.Helpers
{
    using Interfaces;
    using Resources;
    using Xamarin.Forms;

    /***
     * Clase donde almacenará todas las cadenas de texto para traducirlas
     */

    public static class Languages
    {
        static Languages ()
        {
            var ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
            Resource.Culture = ci;
            DependencyService.Get<ILocalize>().SetLocale(ci);
        }

        // LOGIN Y REGISTER
        public static string Accept => Resource.Accept;

        public static string Error => Resource.Error;

        public static string EmailError => Resource.EmailError;

        public static string PasswordError => Resource.PasswordError;

        public static string LoginError => Resource.LoginError;

        public static string Welcome => Resource.Welcome;

        public static string PasswordPlaceHolder => Resource.PasswordPlaceHolder;

        public static string Login => Resource.Login;

        public static string RememberMe => Resource.RememberMe;

        public static string RegisterTitle => Resource.RegisterTitle;

        public static string FirstnamePlaceHolder => Resource.FirstnamePlaceHolder;

        public static string CitySelect => Resource.CitySelect;

        public static string Register => Resource.Register;

        public static string Firstname => Resource.Firstname;

        public static string Username => Resource.Username;

        // ------------------
    }
}