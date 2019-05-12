namespace IPCViewer.Forms.ViewModels
{
    using Common.Models;
    using Common.Services;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Xamarin.Forms;

    public class ProfileViewModel : BaseViewModel
    {
        private readonly ApiService apiService;
        private bool isRunning;
        private bool isEnabled;
        private ObservableCollection<City> cities;
        private City city;
        private User user;

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

    }
}