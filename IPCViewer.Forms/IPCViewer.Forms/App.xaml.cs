namespace IPCViewer.Forms
{
    using System;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;
    using IPCViewer.Forms.Services;
    using IPCViewer.Forms.Views;
    using IPCViewer.Forms.ViewModels;

    public partial class App : Application
    {
        public static NavigationPage Navigator { get; internal set; }
        public static MasterPage Master { get; internal set; }

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
