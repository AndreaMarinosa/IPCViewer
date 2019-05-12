namespace IPCViewer.Forms.ViewModels
{
    using Common.Models;
    using Common.Services;
    using GalaSoft.MvvmLight.Command;
    using IPCViewer.Common.Helpers;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class ProfileViewModel : BaseViewModel
    {
        private readonly ApiService apiService;
        private bool isRunning;
        private bool isEnabled;
        private ObservableCollection<City> cities;
        private City city;
        private User user;

        public ICommand SaveCommand => new RelayCommand(this.Save);

        public ICommand ChangePasswordCommand => new RelayCommand(this.ChangePassword);

        public string CurrentPassword { get; set; }

        public string NewPassword { get; set; }

        public string PasswordConfirm
        {
            get; set;
        }

        public City City
        {
            get => this.city;
            set => this.SetProperty(ref this.city, value);
        }

        public User User
        {
            get => this.user;
            set => this.SetProperty(ref this.user, value);
        }

        public ObservableCollection<City> Cities
        {
            get => this.cities;
            set => this.SetProperty(ref this.cities, value);
        }

        public bool IsRunning
        {
            get => this.isRunning;
            set => this.SetProperty(ref this.isRunning, value);
        }

        public bool IsEnabled
        {
            get => this.isEnabled;
            set => this.SetProperty(ref this.isEnabled, value);
        }

        public ProfileViewModel ()
        {
            this.apiService = new ApiService();
            this.User = MainViewModel.GetInstance().User;
            this.IsEnabled = true;
            LoadCities();
        }
        public async void LoadCities ()
        {

            this.IsRunning = true;
            this.IsEnabled = false;

            var response = await this.apiService.GetListAsync<City>(
                "https://ipcviewerapi.azurewebsites.net",
                "/api",
                "/Cities");

            this.IsRunning = false;
            this.IsEnabled = true;

            if ( !response.IsSuccess || response == null )
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Accept");
                return;
            }

            var myCities = (List<City>) response.Result;
            this.Cities = new ObservableCollection<City>(myCities);

            SetCity();
        }

        private void SetCity ()
        {
            var city = Cities.Where(c => c.Id == this.User.CityId).FirstOrDefault();
            if ( city != null )
            {
                this.City = city;
                return;
            }
        }

        private async void Save ()
        {
            if ( string.IsNullOrEmpty(this.User.FirstName) )
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must enter the first name.",
                    "Accept");
                return;
            }

            if ( string.IsNullOrEmpty(this.User.UserName) )
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must enter the username.",
                    "Accept");
                return;
            }

            if ( this.City == null )
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must select a city.",
                    "Accept");
                return;
            }

            this.IsRunning = true;
            this.IsEnabled = false;

            var response = await this.apiService.PutAsync(
                "https://ipcviewerapi.azurewebsites.net",
                "/api",
                "/Account",
                this.User,
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            this.IsRunning = false;
            this.IsEnabled = true;

            if ( !response.IsSuccess )
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Accept");
                return;
            }

            MainViewModel.GetInstance().User = this.User;
            Settings.User = JsonConvert.SerializeObject(this.User);

            await Application.Current.MainPage.DisplayAlert(
                "Ok",
                "User updated!",
                "Accept");
            await App.Navigator.PopAsync();
        }
      

        private async void ChangePassword ()
        {
            if ( string.IsNullOrEmpty(this.CurrentPassword) )
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must enter the current password.",
                    "Accept");
                return;
            }

            if ( !MainViewModel.GetInstance().UserPassword.Equals(this.CurrentPassword) )
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "The current password is incorrect.",
                    "Accept");
                return;
            }

            if ( string.IsNullOrEmpty(this.NewPassword) )
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must enter the new password.",
                    "Accept");
                return;
            }

            if ( this.NewPassword.Length < 6 )
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "The password must have at least 6 characters length.",
                    "Accept");
                return;
            }

            if ( string.IsNullOrEmpty(this.PasswordConfirm) )
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must enter the password confirm.",
                    "Accept");
                return;
            }

            if ( !this.NewPassword.Equals(this.PasswordConfirm) )
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "The password and confirm does not match.",
                    "Accept");
                return;
            }

            this.IsRunning = true;
            this.IsEnabled = false;

            var request = new ChangePasswordRequest
            {
                Email = MainViewModel.GetInstance().UserEmail,
                NewPassword = this.NewPassword,
                OldPassword = this.CurrentPassword
            };

            var response = await this.apiService.ChangePasswordAsync(
                "https://ipcviewerapi.azurewebsites.net",
                "/api",
                "/Account/ChangePassword",
                request,
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            this.IsRunning = false;
            this.IsEnabled = true;

            if ( !response.IsSuccess )
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Accept");
                return;
            }

            MainViewModel.GetInstance().UserPassword = this.NewPassword;
            Settings.UserPassword = this.NewPassword;

            await Application.Current.MainPage.DisplayAlert(
                "Ok",
                response.Message,
                "Accept");

            await App.Navigator.PopAsync();
        }
    }
}