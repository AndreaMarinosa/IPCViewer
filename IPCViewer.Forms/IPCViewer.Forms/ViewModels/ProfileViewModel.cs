namespace IPCViewer.Forms.ViewModels
{
    using Common.Models;
    using Common.Services;
    using GalaSoft.MvvmLight.Command;
    using IPCViewer.Common.Helpers;
    using IPCViewer.Forms.Helpers;
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
        private bool isDarkMode;

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
        public bool IsDarkMode
        {
            get => this.isDarkMode;
            set => this.SetProperty(ref this.isDarkMode, value);
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
            this.IsDarkMode = MainViewModel.GetInstance().IsDarkMode;
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
                    Languages.Error,
                    Languages.ErrorLoadCities,
                    Languages.Accept);
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
                     Languages.Error,
                    Languages.ErrorEmptyFirstname,
                    Languages.Accept);
                return;
            }

            if ( string.IsNullOrEmpty(this.User.UserName) )
            {
                await Application.Current.MainPage.DisplayAlert(
                     Languages.Error,
                    Languages.ErrorEmptyUsername,
                    Languages.Accept);
                return;
            }

            if ( this.City == null )
            {
                await Application.Current.MainPage.DisplayAlert(
                     Languages.Error,
                    Languages.ErrorCameraCity,
                    Languages.Accept);
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
                   Languages.Error,
                   Languages.ErrorModifyUser,
                   Languages.Accept);
                return;
            }

            MainViewModel.GetInstance().User = this.User;
            Settings.User = JsonConvert.SerializeObject(this.User);

            MainViewModel.GetInstance().IsDarkMode = this.IsDarkMode;
            Settings.IsDarkMode = this.IsDarkMode;

            await Application.Current.MainPage.DisplayAlert(
                "Ok",
               Languages.ModifySuccess,
                   Languages.Accept);
            await App.Navigator.PopAsync();
        }

        private async void ChangePassword ()
        {
            if ( string.IsNullOrEmpty(this.CurrentPassword) )
            {
                await Application.Current.MainPage.DisplayAlert(
                   Languages.Error,
                   Languages.ErrorCurrentPassword,
                   Languages.Accept);
                return;
            }

            if ( !MainViewModel.GetInstance().UserPassword.Equals(this.CurrentPassword) )
            {
                await Application.Current.MainPage.DisplayAlert(
                   Languages.Error,
                   Languages.ErrorCurrentPasswordIncorrect,
                   Languages.Accept);
                return;
            }

            if ( string.IsNullOrEmpty(this.NewPassword) )
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                   Languages.ErrorNewPassword,
                   Languages.Accept);
                return;
            }

            if ( this.NewPassword.Length < 6 )
            {
                await Application.Current.MainPage.DisplayAlert(
                   Languages.Error,
                   Languages.ErrorPasswordLength,
                   Languages.Accept);
                return;
            }

            if ( string.IsNullOrEmpty(this.PasswordConfirm) )
            {
                await Application.Current.MainPage.DisplayAlert(
                   Languages.Error,
                   Languages.ErrorEmptyPasswordConfirm,
                   Languages.Accept);
                return;
            }

            if ( !this.NewPassword.Equals(this.PasswordConfirm) )
            {
                await Application.Current.MainPage.DisplayAlert(
                   Languages.Error,
                   Languages.ErrorPasswordsDoesntMatch,
                   Languages.Accept);
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
                      Languages.Error,
                   Languages.ErrorModifyPassword,
                   Languages.Accept);
                return;
            }

            MainViewModel.GetInstance().UserPassword = this.NewPassword;
            Settings.UserPassword = this.NewPassword;

            await Application.Current.MainPage.DisplayAlert(
                Languages.Error,
                   Languages.ModifyPasswordSuccess,
                   Languages.Accept);

            await App.Navigator.PopAsync();
        }
    }
}