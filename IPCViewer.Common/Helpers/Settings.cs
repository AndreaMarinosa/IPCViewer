﻿namespace IPCViewer.Common.Helpers
{
    // Nuget Xam.Settings
    using Plugin.Settings;
    using Plugin.Settings.Abstractions;

    public static class Settings
    {
        // Almacenamos el token, el usuario, la password y si quiere que le recuerde
        private const string token = "token"; // leemos el token como string para posteriormente serializarlo y desserializarlo como un objeto
        private const string userEmail = "userEmail";
        private const string userPassword = "userPassword";
        private const string isRemember = "isRemember";
        private static readonly string stringDefault = string.Empty; // Defino los valores por defecto. Por defecto esta vacio
        private static readonly bool boolDefault = false;

        private static ISettings AppSettings => CrossSettings.Current;

        // Por cada constante, se define una propiedad publica y estatica
        public static string Token
        {
            get => AppSettings.GetValueOrDefault(token, stringDefault);
            set => AppSettings.AddOrUpdateValue(token, value);
        }

        public static string UserEmail
        {
            get => AppSettings.GetValueOrDefault(userEmail, stringDefault);
            set => AppSettings.AddOrUpdateValue(userEmail, value);
        }

        public static string UserPassword
        {
            get => AppSettings.GetValueOrDefault(userPassword, stringDefault);
            set => AppSettings.AddOrUpdateValue(userPassword, value);
        }

        public static bool IsRemember
        {
            get => AppSettings.GetValueOrDefault(isRemember, boolDefault);
            set => AppSettings.AddOrUpdateValue(isRemember, value);
        }
    }

}