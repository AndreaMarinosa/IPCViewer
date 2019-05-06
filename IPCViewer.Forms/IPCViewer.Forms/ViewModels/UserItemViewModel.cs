namespace IPCViewer.Forms.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Common.Models;
    using System.Windows.Input;
    using Views;

    /**
     * Clase para meter un comando en el modelo 'User'
     * Se utiliza esta nueva clase para no meter codigo interno
     * al modelo de camara, de esta manera se conserva el modelo limpio
     */
    public class UserItemViewModel : User
    {
        public ICommand SelectCameraCommand => new RelayCommand(SelectUser);


        private async void SelectUser()
        {
            MainViewModel.GetInstance().EditUser = new EditUserViewModel(this);
            await App.Navigator.PushAsync(new EditUserPage());
        }
    }
}