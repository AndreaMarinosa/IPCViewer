namespace IPCViewer.Forms.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Common.Helpers;
    using Common.Models;
    using Common.Services;
    using Views;
    using Newtonsoft.Json;
    using System.Windows.Input;
    using Xamarin.Forms;

    class LoginViewModel : BaseViewModel
    {

        private bool _isRunning;
        private bool _isEnabled;
        private readonly ApiService apiService;

        #region Properties

        public bool IsRemember { get; set; }

        public bool IsRunning
        {
            get => this._isRunning;
            set => this.SetProperty(ref _isRunning, value);
        }

        public bool IsEnabled
        {
            get => this._isEnabled;
            set => this.SetProperty(ref _isEnabled, value);
        }

        public string Email { get; set; }

        public string Password
        {
            get; set;

        }
        #endregion

        public ICommand LoginCommand => new RelayCommand(this.Login);

        public LoginViewModel()
        {
            apiService = new ApiService();
            IsEnabled = true;
            Email = "andreamarinosalopez@gmail.com";
            Password = "123456";
            IsRemember = true;

        }


        private async void Login()
        {
            if (string.IsNullOrEmpty(this.Email))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "You must enter an email", "Accept");
                return;
            }
            if (string.IsNullOrEmpty(this.Password))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "You must enter a password", "Accept");
                return;
            }

            IsRunning = true;
            IsEnabled = false;

            var request = new TokenRequest
            {
                Password = this.Password,
                Username = this.Email
            };


            //var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetTokenAsync(
                "https://ipcviewerapi.azurewebsites.net",
                "/api",
                "/Account/PostCreateToken",
                request);

            IsRunning = false;
            IsEnabled = true;


            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Accept");
                return;
            }

            var token = (TokenResponse)response.Result;
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.Token = token;
            mainViewModel.Cameras = new CamerasViewModel();
            mainViewModel.UserEmail = this.Email;
            mainViewModel.UserPassword = this.Password;


            // Guardamos los datos en persistencia (El email, la password, y el remember)
            Settings.IsRemember = IsRemember;
            Settings.UserEmail = Email;
            Settings.UserPassword = Password;
            Settings.Token = JsonConvert.SerializeObject(token); // coge el token y lo guarda en un string

            Application.Current.MainPage = new /*NavigationPage(new */MasterPage()/*)*/;

        }
    }
}