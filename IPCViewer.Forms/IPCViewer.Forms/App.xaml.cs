﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using IPCViewer.Forms.Services;
using IPCViewer.Forms.Views;

namespace IPCViewer.Forms
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new ControlUsersPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
