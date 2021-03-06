﻿namespace IPCViewer.Forms
{
    using IPCViewer.Common.Helpers;
    using IPCViewer.Common.Models;
    using Newtonsoft.Json;
    using System;
    using ViewModels;
    using Views;
    using Xamarin.Forms;

    public partial class App : Application
    {
        // Para navegar por la aplicacion
        public static NavigationPage Navigator { get; internal set; }

        public static MasterPage Master { get; internal set; }

        public App ()
        {
            //Register Syncfusion license
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("@31372e312e30Yq+maR8+yovkjCxAUy9Of06jRxlGClixfzVZKnNjTrE=");

            InitializeComponent();

            // Cargamos primero los settings del usuario
            if ( Settings.IsRemember )
            {
                //if ( !Settings.IsDarkMode )
                //{
                //    Resources["ContentPageStyle"] = Resources["DarkMode"];
                //    Console.WriteLine("Funciona");
                //}
                //else
                //{
                //    Console.WriteLine("NO Funciona");
                //    Resources["ContentPageStyle"] = Resources["LightMode"];
                //}

                // Como esta guardado como un string, lo deserilizamos como un objeto
                var token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);

                // Si el usuario esta recordado, lo deserializamos para volverlo objeto
                var user = JsonConvert.DeserializeObject<User>(Settings.User);

                // Si el token no es valido (Porque ha pasado el tiempo), lo devolvemos al login
                if ( token.Expiration > DateTime.Now )
                {
                    var mainViewModel = MainViewModel.GetInstance();
                    mainViewModel.Token = token;
                    mainViewModel.User = user;
                    mainViewModel.UserEmail = Settings.UserEmail;
                    mainViewModel.UserPassword = Settings.UserPassword;
                    mainViewModel.Cameras = new CamerasViewModel();
                    mainViewModel.IsDarkMode = Settings.IsDarkMode;
                    this.MainPage = new MasterPage();
                    return;
                }
            }

           

            //MainViewModel.GetInstance().ControlUsersViewModel = new ControlUsersViewModel();
            MainPage = /*new NavigationPage(*/new ControlUsersPage()/*)*/;
        }

        protected override void OnStart ()
        {
            // Handle when your app starts
        }

        protected override void OnSleep ()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume ()
        {
            // Handle when your app resumes
        }
    }
}