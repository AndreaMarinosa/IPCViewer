

namespace IPCViewer.Forms.ViewModels
{

    using GalaSoft.MvvmLight.Command;
    using IPCViewer.Common.Models;
    using IPCViewer.Common.Services;
    using IPCViewer.Forms.Views;
    using System.Windows.Input;
    using Xamarin.Forms;

    class LoginViewModel : BaseViewModel
    {

        

        private bool _isRunning;
        private bool _isEnabled;
        private readonly ApiService apiService;

        #region Properties
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

            this.IsRunning = true;
            this.IsEnabled = false;

            var request = new TokenRequest
            {
                Password = this.Password,
                Username = this.Email
            };


            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetTokenAsync(
                url,
                "/Account",
                "/CreateToken",
                request);

            this.IsRunning = false;
            this.IsEnabled = true;


            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Email or password incorrect.", "Accept");
                return;
            }



            var token = (TokenResponse)response.Result;
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.Token = token;
            mainViewModel.Cameras = new CamerasViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new CamerasPage());

        }
    }
}