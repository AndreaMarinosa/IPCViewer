using System;
using System.Collections.Generic;
using IPCViewer.Forms.Interfaces;

namespace IPCViewer.Forms.ViewModels
{
    using Common.Models;
    using Common.Services;
    using GalaSoft.MvvmLight.Command;
    using IPCViewer.Common.Helpers;
    using IPCViewer.Forms.Views;
    using Plugin.Media;
    using Plugin.Media.Abstractions;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class AddCameraViewModel : BaseViewModel, IClosePopup, ILocation
    {
        private bool isRunning;
        private bool isEnabled;
        private bool isVisible;
        private string _latitude;
        private string _longitude;
        private ApiService apiService;
        private ObservableCollection<City> cities;
        private City city;
        private MediaFile file;
        private ImageSource _imageSource;
        private string urlCamera;


        public string Name { get; set; }

        public string Comments { get; set; }

        public string Latitude
        {
            get => this._latitude;
            set => this.SetProperty(ref this._latitude, value);
        }

        public string Longitude
        {
            get => this._longitude;
            set => this.SetProperty(ref this._longitude, value);
        }

        public ImageSource ImageSource
        {
            get => this._imageSource;
            set => this.SetProperty(ref this._imageSource, value);
        }

        public string UrlCamera
        {
            get => this.urlCamera;
            set => SetProperty(ref this.urlCamera, value);
        }

        public ICommand AddLocationCommand => new RelayCommand(this.AddLocation);

        public ICommand SaveCommand => new RelayCommand(this.Save);

        public ICommand ChangeImageCommand => new RelayCommand(this.ChangeImage);

        public bool IsRunning { get => this.isRunning; set => this.SetProperty(ref this.isRunning, value); }

        public bool IsEnabled { get => this.isEnabled; set => this.SetProperty(ref this.isEnabled, value); }

        public City City { get => this.city; set => this.SetProperty(ref this.city, value); }

        public ObservableCollection<City> Cities { get => this.cities; set => this.SetProperty(ref this.cities, value); }

        public bool IsVisible { get => this.isVisible; set => this.SetProperty(ref this.isVisible, value); }


        public AddCameraViewModel ()
        {
            this.apiService = new ApiService();
            this.ImageSource = "noImage";
            LoadCities();
            this.IsEnabled = true;
        }

        public async void LoadCities ()
        {

            IsRunning = true;
            IsEnabled = false;

            var response = await this.apiService.GetListAsync<City>(
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
            this.Cities = new ObservableCollection<City>(myCities);

        }

        private async void Save ()
        {
            if ( string.IsNullOrEmpty(Name) )
            {
                await Application.Current.MainPage.DisplayAlert("Error", "You must enter a camera name.", "Accept");
                return;
            }

            if ( string.IsNullOrEmpty(Latitude) )
            {
                await Application.Current.MainPage.DisplayAlert("Error", "You must enter a latitude.", "Accept");
                return;
            }

            if ( string.IsNullOrEmpty(Longitude) )
            {
                await Application.Current.MainPage.DisplayAlert("Error", "You must enter a longitude.", "Accept");
                return;
            }

            var latitude = double.Parse(this.Latitude);
            var longitude = double.Parse(this.Longitude);

            this.IsRunning = true;
            this.IsEnabled = false;

            var camera = new Camera
            {
                Name = this.Name,
                //ImageArray = ImageSource,
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

            byte[] imageArray = null;
            // si es file no es null, que cargue la imagen
            if ( this.file != null )
            {
                imageArray = FilesHelper.ReadFully(this.file.GetStream());
                camera.ImageArray = imageArray;

            }
            // si el file image array es null, que compruebe si existe la url
            else if (!string.IsNullOrEmpty(UrlCamera))
            {
                camera.ImageUrl = UrlCamera;

            }
            else{
                await Application.Current.MainPage.DisplayAlert("Error", "You must enter an url or image.", "Accept");
                return;
            }

            // si no tiene ninguna, poner un alert y que cancele la operacion


            var response = await apiService.PostAsync(
                "https://ipcviewerapi.azurewebsites.net",
                "/api",
                "/Cameras",
                camera,
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            if ( !response.IsSuccess )
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Accept");

                this.IsRunning = false;
                this.IsEnabled = true;
                return;
            }

            var newCamera = (Camera) response.Result;
            MainViewModel.GetInstance().Cameras.AddCamera(newCamera);

            this.IsRunning = false;
            this.IsEnabled = true;
            await App.Navigator.PopAsync();
        }

        private async void ChangeImage ()
        {
            // Inicializamos la camara
            await CrossMedia.Current.Initialize();

            // Dialogo para varias opciones
            var source = await Application.Current.MainPage.DisplayActionSheet(
                "Where do you take the picture?",
                "Cancel",
                null,
                "From Gallery",
                "From Camera",
                "From Url");

            switch (source)
            {
                case "Cancel":
                    {
                        this.file = null;
                        return;
                    }
                case "From Camera":
                    {
                        // le decimos que coja la foto de la camara
                        this.file = await CrossMedia.Current.TakePhotoAsync(
                            new StoreCameraMediaOptions
                            {
                                Directory = "Pictures",
                                Name = "test.jpg",
                                PhotoSize = PhotoSize.Small,
                            }
                        );
                        break;
                    }
                case "From Galery":
                    {
                        this.file = await CrossMedia.Current.PickPhotoAsync();
                        break;
                    }
                case "From Url":
                    {
                        // todo: aniadir popup, crear propertie image url y comprobar si es null
                        MainViewModel.GetInstance().AddUrl = new AddUrlViewModel(this);
                        await App.Navigator.PushAsync(new AddUrlPage());
                        break;
                    }
               
            }

            // Si han elegido una imagen
            if ( this.file != null )
            {
                this.ImageSource = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    return stream;
                });
            }

        }

        // todo: add location from maps
        private async void AddLocation ()
        {
            MainViewModel.GetInstance().AddLocation = new AddLocationViewModel(this);
            await App.Navigator.PushAsync(new AddLocationPage());
        }


        public void OnClose(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                IsVisible = false;
            }
            else
            {
                UrlCamera = url;
                IsVisible = true;
            }
        }

        public void SetLocation(string longitude, string latitude, ImageSource imageSource)
        {
            Latitude = latitude;
            Longitude = longitude;
            ImageSource = imageSource;
        }
    }
}