using System;
using System.Collections.Generic;

namespace IPCViewer.Forms.ViewModels
{
    using Common.Models;
    using Common.Services;
    using GalaSoft.MvvmLight.Command;
    using IPCViewer.Common.Helpers;
    using Plugin.Media;
    using Plugin.Media.Abstractions;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class AddCameraViewModel : BaseViewModel
    {
        private bool isRunning;
        private bool isEnabled;
        private ApiService apiService;
        private ObservableCollection<City> cities;
        private City city;
        private MediaFile file;
        private ImageSource imageSource;


        public string Name { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }

        public string Comments { get; set; }

        public ImageSource ImageSource
        {
            get => this.imageSource;
            set => this.SetProperty(ref this.imageSource, value);
        }

        public string UrlCamera { get; set; }

        public ICommand SaveCommand => new RelayCommand(this.Save);

        public ICommand ChangeImageCommand => new RelayCommand(this.ChangeImage);

        public bool IsRunning { get => this.isRunning; set => this.SetProperty(ref this.isRunning, value); }

        public bool IsEnabled { get => this.isEnabled; set => this.SetProperty(ref this.isEnabled, value); }

        public City City { get => this.city; set => this.SetProperty(ref this.city, value); }

        public ObservableCollection<City> Cities { get => this.cities; set => this.SetProperty(ref this.cities, value); }

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
            // todo: more alerts
            if ( string.IsNullOrEmpty(Name) )
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
            else
            {
                // image url
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

            if ( source == "Cancel" )
            {
                this.file = null;
                return;
            }

            if ( source == "From Camera" )
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
            }
            // desde la galeria
            else if ( source == "From Gallery" )
            {
                this.file = await CrossMedia.Current.PickPhotoAsync();
            }
            // desde url
            else
            {
                // todo: aniadir popup, crear propertie image url y comprobar si es null
            }

            if ( this.file != null )
            {
                this.ImageSource = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    return stream;
                });
            }
        }

    }
}