using System;
using System.Collections.Generic;
using System.Text;

namespace IPCViewer.Forms.ViewModels
{
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

        public string Image { get; set; }

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

        public string Name { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }

        public string Comments { get; set; }

        public ICommand SaveCommand => new RelayCommand(this.Save);

        public AddCameraViewModel()
        {
            this.apiService = new ApiService();
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

            if(latitude <= 0.00)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "The latitude must be a number greather than zero.", "Accept");
            }
            if(longitude <= 0.00)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "The longitude must be a number greather than zero.", "Accept");
            }

            this.IsRunning = true;
            this.IsEnabled = false;

            var camera = new Camera
            {
                Name = this.Name,
                Comments = this.Comments,
                City = new City { Name = "Zaragoza" },
                Latitude = latitude,
                Longitude = longitude,
                User = new User
                {
                    UserName = MainViewModel.GetInstance().UserEmail
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
                return;
            }

            var newCamera = (Camera)response.Result;
            MainViewModel.GetInstance().Cameras.Cameras.Add(newCamera);

            this.IsRunning = false;
            this.IsEnabled = true;
            await Application.Current.MainPage.Navigation.PopAsync();
        }

    }
}