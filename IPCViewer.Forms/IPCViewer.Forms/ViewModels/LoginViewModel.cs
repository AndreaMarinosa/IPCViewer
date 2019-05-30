namespace IPCViewer.Forms.ViewModels
{
    using Common.Helpers;
    using Common.Models;
    using Common.Services;
    using GalaSoft.MvvmLight.Command;
    using IPCViewer.Forms.Helpers;
    using Newtonsoft.Json;
    using System.Windows.Input;
    using Views;
    using Xamarin.Forms;

    public class LoginViewModel : BaseViewModel
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

        #endregion Properties

        public ICommand LoginCommand => new RelayCommand(this.Login);

        public LoginViewModel ()
        {
            apiService = new ApiService();
            IsEnabled = true;
            Email = "andreamarinosalopez@gmail.com";
            Password = "123456";
            IsRemember = true;
        }

        private async void Login ()
        {
            if ( string.IsNullOrEmpty(this.Email) )
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.EmailError,
                    Languages.Accept);
                return;
            }
            if ( string.IsNullOrEmpty(this.Password) )
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.PasswordError,
                    Languages.Accept);
                return;
            }

            IsRunning = true;
            IsEnabled = false;

            var request = new TokenRequest
            {
                Password = this.Password,
                Username = this.Email
            };

            var response = await this.apiService.GetTokenAsync(
                "https://ipcviewerapi.azurewebsites.net",
                "/api",
                "/Account/PostCreateToken",
                request);

            IsRunning = false;
            IsEnabled = true;

            if ( !response.IsSuccess )
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.LoginError,
                    Languages.Accept);
                return;
            }

            var token = (TokenResponse) response.Result;

            // Cogemos las datos del usuario mediante ese email
            var response2 = await this.apiService.GetUserByEmailAsync(
                "https://ipcviewerapi.azurewebsites.net",
                "/api",
                "/Account/GetUserByEmail",
                this.Email,
                "bearer",
                token.Token);

            var user = (User) response2.Result;

            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.User = user;
            mainViewModel.Token = token;
            mainViewModel.Cameras = new CamerasViewModel();
            mainViewModel.UserEmail = this.Email;
            mainViewModel.UserPassword = this.Password;
            mainViewModel.IsDarkMode = Settings.IsDarkMode;

            // Guardamos los datos en persistencia (El email, la password, y el remember)
            Settings.IsRemember = IsRemember;
            Settings.UserEmail = Email;
            Settings.UserPassword = Password;
            Settings.Token = JsonConvert.SerializeObject(token); // coge el token y lo guarda en un string
            Settings.User = JsonConvert.SerializeObject(user); // coge el user y lo guarda en un string
            

            Application.Current.MainPage = new /*NavigationPage(new */MasterPage()/*)*/;
        }
    }
}