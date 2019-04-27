using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using IPCViewer.Forms.Services;
using IPCViewer.Forms.Views;
using IPCViewer.Forms.ViewModels;

namespace IPCViewer.Forms
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainViewModel.GetInstance().ControlUsersPage = new ControlUsersPage();
            MainPage = new NavigationPage(new ControlUsersPage());
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
