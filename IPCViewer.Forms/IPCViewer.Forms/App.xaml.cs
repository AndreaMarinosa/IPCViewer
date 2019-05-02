namespace IPCViewer.Forms
{
    using ViewModels;
    using Views;
    using Xamarin.Forms;

    public partial class App : Application
    {
        public static NavigationPage Navigator { get; internal set; }
        public static MasterPage Master { get; internal set; }

        public App()
        {
            InitializeComponent();

            MainViewModel.GetInstance().ControlUsersPage = new ControlUsersPage();
            MainPage = /*new NavigationPage(*/new ControlUsersPage()/*)*/;
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
