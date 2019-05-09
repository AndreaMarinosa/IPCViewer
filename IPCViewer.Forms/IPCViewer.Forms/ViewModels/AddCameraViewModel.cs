namespace IPCViewer.Forms.ViewModels
{

    using System;
    using System.Collections.Generic;
    using Common.Models;
    using Common.Services;
    using GalaSoft.MvvmLight.Command;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
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

        public ICommand SaveCommand => new RelayCommand(Save);

        public bool IsRunning
        {
            get => isRunning;
            set => SetProperty(ref isRunning, value);
        }

        public bool IsEnabled
        {
            get => isEnabled;
            set => SetProperty(ref isEnabled, value);
        }

        public City City
        {
            get => city;
            set => SetProperty(ref city, value);
        }

        public ObservableCollection<City> Cities
        {
            get => cities;
            set => SetProperty(ref cities, value);
        }

        public async void LoadCities ()
        {

            IsRunning = true;
            IsEnabled = false;

            //var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await apiService.GetListAsync<City>(
                "https://ipcviewerapi.azurewebsites.net",
                "/api",
                "/Cities");

            IsRunning = false;
            IsEnabled = true;

            if ( !response.IsSuccess )
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Accept");
                return;
            }

            var myCities = (List<City>) response.Result;
            Cities = new ObservableCollection<City>(myCities);

        }

        public AddCameraViewModel ()
        {
            apiService = new ApiService();
            LoadCities();
            Image = "noImage";
            IsEnabled = true;
        }

        private async void Save ()
        {
            if ( string.IsNullOrEmpty(Name) )
            {
                await Application.Current.MainPage.DisplayAlert("Error", "You must enter a camera name.", "Accept");
                return;
            }

            var latitude = double.Parse(Latitude);
            var longitude = double.Parse(Longitude);

            IsRunning = true;
            IsEnabled = false;

            var camera = new Camera
            {
                Name = Name,
                ImageUrl = ImageUrl,
                Comments = Comments,
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

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await apiService.PostAsync(
                url,
                "/api",
                "/Cameras",
                camera,
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            if ( !response.IsSuccess )
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Accept");

                IsRunning = false;
                IsEnabled = true;
                return;
            }

            var newCamera = (Camera) response.Result;
            MainViewModel.GetInstance().Cameras.AddCamera(newCamera);

            IsRunning = false;
            IsEnabled = true;
            await App.Navigator.PopAsync();
        }

    }
}