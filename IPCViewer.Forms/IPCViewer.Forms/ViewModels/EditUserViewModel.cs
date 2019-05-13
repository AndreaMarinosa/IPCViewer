namespace IPCViewer.Forms.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using IPCViewer.Common.Models;
    using IPCViewer.Common.Services;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class EditUserViewModel : BaseViewModel
    {
        private bool isRunning;
        private bool isEnabled;
        private readonly ApiService apiService;

        public User User { get; set; }

        public bool IsRunning
        {
            get => this.isRunning;
            set => this.SetProperty(ref this.isRunning, value);
        }

        //public ICommand SaveCommand => new RelayCommand(this.Save);

        public ICommand DeleteCommand => new RelayCommand(this.Delete);

        public bool IsEnabled
        {
            get => this.isEnabled;
            set => this.SetProperty(ref this.isEnabled, value);
        }

        /**
         * La camara ligada a la MainViewModel es la que se pasa por parametro
         */

        public EditUserViewModel (User user)
        {
            this.User = user;
            this.apiService = new ApiService();
            this.IsEnabled = true;
        }

        //private async void Save()
        //{
        //    // todo: poner mas alerts para las propiedades
        //    if (string.IsNullOrEmpty(this.User.Email))
        //    {
        //        await Application.Current.MainPage.DisplayAlert("Error", "You must enter a user name.", "Accept");
        //        return;
        //    }

        //    // Todo: other alerts

        //    this.IsRunning = true;
        //    this.IsEnabled = false;

        //    var response = await this.apiService.PutUserAsync(
        //        "https://ipcviewerapi.azurewebsites.net",
        //        "/api",
        //        "/Account",
        //        User.Id,
        //        User);

        //    this.IsRunning = false;
        //    this.IsEnabled = true;
        //    if (!response.IsSuccess)
        //    {
        //        await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Accept");
        //        return;
        //    }

        //    var modifiedUser = (User)response.Result;
        //    MainViewModel.GetInstance().Users.UpdateUser(modifiedUser);
        //    await App.Navigator.PopAsync();

        //}

        private async void Delete ()
        {
            var confirm = await Application.Current.MainPage.DisplayAlert("Confirm", "Are you sure to delete the user?", "Yes", "No");
            if ( !confirm )
            {
                return;
            }

            this.IsRunning = true;
            this.IsEnabled = false;

            var response = await this.apiService.DeleteUserAsync(
                "https://ipcviewerapi.azurewebsites.net",
                "/api",
                "/Account",
                this.User.Id);

            this.IsRunning = false;
            this.IsEnabled = true;

            if ( !response.IsSuccess )
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Accept");
                return;
            }

            MainViewModel.GetInstance().Users.DeleteUser(this.User.Id);
            await App.Navigator.PopAsync();
        }
    }
}