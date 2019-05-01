using System;
using System.Collections.Generic;
using System.Text;

namespace IPCViewer.Forms.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using Common.Models;
    using Common.Services;
    using GalaSoft.MvvmLight.Command;
    using Xamarin.Forms;

    public class AddCameraViewModel : BaseViewModel
    {
        private bool isRunning;
        private bool isEnabled;
        private readonly ApiService apiService;
        private ObservableCollection<City> cities;
        private City city;

        public string Image { get; set; }

        public string Name { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }

        public string Comments { get; set; }

        public string ImageUrl { get; set; }

        public string CityId { get; set; }

        public ICommand SaveCommand => new RelayCommand(this.Save);

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

        public async void LoadCities()
        {

            IsRunning = true;
            IsEnabled = false;

            //var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetListAsync<City>(
                "https://ipcviewerapi.azurewebsites.net",
                "/api",
                "/Cities");

            IsRunning = false;
            IsEnabled = true;

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

        public AddCameraViewModel()
        {
            this.apiService = new ApiService();
            LoadCities();
            this.Image = "noImage";
            this.IsEnabled = true;
        }

        private async void Save()
        {
            if (string.IsNullOrEmpty(Name))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "You must enter a camera name.", "Accept");
                return;
            }

            var latitude = double.Parse(this.Latitude);
            var longitude = double.Parse(this.Longitude);

            this.IsRunning = true;
            this.IsEnabled = false;

            var camera = new Camera
            {
                Name = this.Name,
                ImageUrl = ImageUrl,
                Comments = this.Comments,
                CityId = City.Id,
                City = City,
                Latitude = latitude,
                Longitude = longitude,
                User = new User
                {
                    Email = MainViewModel.GetInstance().UserEmail
                },
                CreatedDate = DateTime.Now,
            };

            //var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.PostAsync(
                "https://ipcviewerapi.azurewebsites.net",
                "/api",
                "/Cameras",
                camera,
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Accept");

                this.IsRunning = false;
                this.IsEnabled = true;
                return;
            }

            var newCamera = (Camera)response.Result;
            MainViewModel.GetInstance().Cameras.Cameras.Add(newCamera);

            this.IsRunning = false;
            this.IsEnabled = true;
            await App.Navigator.PopAsync();
        }

    }
}