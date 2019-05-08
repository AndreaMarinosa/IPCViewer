namespace IPCViewer.Forms.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using Common.Models;
    using GalaSoft.MvvmLight.Command;
    using IPCViewer.Common.Helpers;
    using IPCViewer.Common.Services;
    using Xamarin.Forms;

    public class RegisterViewModel : BaseViewModel
    {
        private bool isRunning;
        private bool isEnabled;
        private ObservableCollection<City> cities;
        private City city;

        public string FirstName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        private readonly ApiService apiService;


        public City City
        {
            get => this.city;
            set => this.SetProperty(ref this.city, value);
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

        public ICommand RegisterCommand => new RelayCommand(this.Register);

        public async void LoadCities()
        {

            this.IsRunning = true;
            this.IsEnabled = false;

            //var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetListAsync<City>(
                "https://ipcviewerapi.azurewebsites.net",
                "/api",
                "/Cities");

            this.IsRunning = false;
            this.IsEnabled = true;

            if (!response.IsSuccess || response == null)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Accept");
                return;
            }

            var myCities = (List<City>)response.Result;
            this.Cities = new ObservableCollection<City>(myCities);
        }

        public RegisterViewModel()
        {
            this.apiService = new ApiService();
            this.IsEnabled = true;
            LoadCities();
            FirstName = "Andrea2";
            Email = "q@shitmail.me";
            Password = "123456";
        }


        private async void Register()
        {
            if (string.IsNullOrEmpty(this.FirstName))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must enter the first name.",
                    "Accept");
                return;
            }

            
            if (string.IsNullOrEmpty(this.Email))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must enter an email.",
                    "Accept");
                return;
            }

            if (!RegexHelper.IsValidEmail(this.Email))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must enter a valid email.",
                    "Accept");
                return;
            }

            if (this.City == null)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must select a city.",
                    "Accept");
                return;
            }

            if (string.IsNullOrEmpty(this.Password))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must enter a password.",
                    "Accept");
                return;
            }

            if (this.Password.Length < 6)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "You password must be at mimimun 6 characters.",
                    "Accept");
                return;
            }

            // TODO: More alerts
            this.IsRunning = true;
            this.IsEnabled = false;

            var request = new NewUserRequest
            {
                CityId = this.City.Id,
                Email = this.Email,
                FirstName = this.FirstName,
                Password = this.Password,
                UserName = this.Email
            };

            //var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.RegisterUserAsync(
                "https://ipcviewerapi.azurewebsites.net",
                "/api",
                "/Account/PostUser",
                request);

            this.IsRunning = false;
            this.IsEnabled = true;

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Accept");
                return;
            }

            await Application.Current.MainPage.DisplayAlert(
                "Ok",
                response.Message,
                "Accept");
            await App.Navigator.PopAsync();
        }

    }

}
