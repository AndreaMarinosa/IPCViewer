namespace IPCViewer.Forms.ViewModels
{
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using IPCViewer.Common.Helpers;
    using Views;
    using Xamarin.Forms;

    public class MenuItemViewModel : Common.Models.Menu
    {
        public ICommand SelectMenuCommand => new RelayCommand(SelectMenu);

        private async void SelectMenu()
        {
            var mainViewModel = MainViewModel.GetInstance();
            App.Master.IsPresented = false;

            switch (PageName)
            {
                case "AboutPage":
                    await App.Navigator.PushAsync(new AboutPage());
                    break;
                case "SetupPage":
                    await App.Navigator.PushAsync(new SetupPage());
                    break;

                case "MapsPage":
                    //MainViewModel.GetInstance().Map = new MapViewModel();
                    await App.Navigator.PushAsync(new MapsPage());
                    break;

                case "UsersPage":
                    MainViewModel.GetInstance().Users = new UsersViewModel();
                    await App.Navigator.PushAsync(new UsersPage());
                    break;

                default:
                    // Cuando cierre sesion el usuario, que se quiten los valores de persistencia
                    Settings.IsRemember = false;
                    Settings.Token = string.Empty;
                    Settings.UserEmail = string.Empty;
                    Settings.UserPassword = string.Empty;

                    MainViewModel.GetInstance().ControlUsersPage = new ControlUsersPage();
                    Application.Current.MainPage = new /*NavigationPage(new */ControlUsersPage(/*)*/);
                    break;
            }
        }
    }

}
