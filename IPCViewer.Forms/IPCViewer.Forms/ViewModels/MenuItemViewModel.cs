namespace IPCViewer.Forms.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using IPCViewer.Common.Helpers;
    using System.Windows.Input;
    using Views;
    using Xamarin.Forms;

    public class MenuItemViewModel : Common.Models.Menu
    {
        public ICommand SelectMenuCommand => new RelayCommand(SelectMenu);

        private async void SelectMenu ()
        {
            var mainViewModel = MainViewModel.GetInstance();
            App.Master.IsPresented = false;

            switch ( PageName )
            {
                case "AboutPage":
                    await App.Navigator.PushAsync(new AboutPage());
                    break;

                case "ProfilePage":
                    MainViewModel.GetInstance().Profile = new ProfileViewModel();
                    await App.Navigator.PushAsync(new ProfilePage());
                    break;

                case "MapsPage":
                    MainViewModel.GetInstance().Maps = new MapViewModel();
                    await App.Navigator.PushAsync(new MapsPage());
                    break;

                default:
                    // Cuando cierre sesion el usuario, que se quiten los valores de persistencia
                    Settings.User = string.Empty;
                    Settings.IsRemember = false;
                    Settings.Token = string.Empty;
                    Settings.UserEmail = string.Empty;
                    Settings.UserPassword = string.Empty;

                    //MainViewModel.GetInstance().ControlUsersViewModel = new ControlUsersViewModel();
                    Application.Current.MainPage = new ControlUsersPage();
                    break;
            }
        }
    }
}