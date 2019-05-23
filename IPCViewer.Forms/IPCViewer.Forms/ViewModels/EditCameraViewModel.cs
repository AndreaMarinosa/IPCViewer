using IPCViewer.Common.Helpers;

namespace IPCViewer.Forms.ViewModels
{
    using System;
    using System.Windows.Input;
    using Common.Models;
    using Common.Services;
    using GalaSoft.MvvmLight.Command;
    using Interfaces;
    using Views;
    using Xamarin.Forms;
    using Plugin.Media;
    using Plugin.Media.Abstractions;
    using System.IO;
    using IPCViewer.Forms.Helpers;

    public class EditCameraViewModel : BaseViewModel, ILocation, IClosePopup
    {
        private readonly ApiService apiService;
        private bool isEnabled;
        private bool isRunning;
        private string latitude;
        private string longitude;
        private MediaFile file;
        private ImageSource imageSource;
        private byte[] _imageByte;
        private string _urlCamera;

        public string Latitude
        {
            get { return latitude; }
            set { SetProperty(ref latitude, value); }
        }

        public string Longitude
        {
            get { return longitude; }
            set { SetProperty(ref longitude, value); }
        }

        public ImageSource ImageSource
        {
            get { return imageSource; }
            set { SetProperty(ref imageSource, value); }
        }

        public bool IsEnabled
        {
            get { return isEnabled; }
            set { SetProperty(ref isEnabled, value); }
        }

        public bool IsRunning
        {
            get { return isRunning; }
            set { SetProperty(ref isRunning, value); }
        }

        public string UrlCamera
        {
            get { return _urlCamera; }
            set { SetProperty(ref _urlCamera, value); }
        }

        public Camera Camera { get; set; }

        public ICommand AddLocationCommand
        {
            get { return new RelayCommand(AddLocation); }
        }

        public ICommand DeleteCommand
        {
            get { return new RelayCommand(Delete); }
        }

        public ICommand SaveCommand
        {
            get { return new RelayCommand(Save); }
        }

        public ICommand TapCommand
        {
            get { return new RelayCommand(ChangeImage); }
        }

        /**
         * La camara ligada a la MainViewModel es la que se pasa por parametro
         */
        public EditCameraViewModel (Camera camera)
        {
            Camera = camera;
            apiService = new ApiService();
            IsEnabled = true;
            Longitude = Camera.Longitude.ToString();
            Latitude = Camera.Latitude.ToString();
            UrlCamera = camera.ImageUrl;
            ImageSource = camera.ImageFullPath;
        }


        private async void Delete ()
        {
            var confirm = await Application.Current.MainPage.DisplayAlert("Confirm", "Are you sure to delete the camera?", "Yes", "No");
            if ( !confirm )
            {
                return;
            }

            IsRunning = true;
            IsEnabled = false;

            var response = await apiService.DeleteAsync(
                "https://ipcviewerapi.azurewebsites.net",
                "/api",
                "/Cameras",
                Camera.Id,
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            IsRunning = false;
            IsEnabled = true;

            if ( !response.IsSuccess )
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Accept");
                return;
            }

            MainViewModel.GetInstance().Cameras.DeleteCamera(Camera.Id);
            await App.Navigator.PopAsync();
        }

        private async void Save ()
        {
            if ( string.IsNullOrEmpty(Camera.Name) )
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.ErrorCameraName,
                    Languages.Accept);
                return;
            }

            if ( Camera.City == null )
            {
                await Application.Current.MainPage.DisplayAlert(
                     Languages.Error,
                    Languages.ErrorCameraCity,
                    Languages.Accept);
                return;
            }

            if ( string.IsNullOrEmpty(Latitude) )
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.ErrorCameraLatitude,
                    Languages.Accept);
                return;
            }

            if ( string.IsNullOrEmpty(Longitude) )
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                     Languages.ErrorCameraLongitude,
                    Languages.Accept);
                return;
            }

            byte[] imageArray = null;
            // si es file no es null, que cargue la imagen
            if ( this.file != null )
            {
                imageArray = FilesHelper.ReadFully(this.file.GetStream());
                Camera.ImageArray = imageArray;
            }
            // si el file image array es null, que compruebe si existe la url
            else if ( !string.IsNullOrEmpty(UrlCamera) )
            {
                Camera.ImageUrl = UrlCamera;
            }
            // si el screenshot no es null
            else if ( this._imageByte != null )
            {
                Camera.ImageArray = _imageByte;
            }
            else
            {
                var source = await Application.Current.MainPage.DisplayAlert(
                   Languages.Alert,
                   Languages.AlertImageCamera,
                   Languages.Accept,
                   Languages.Cancel);
                if ( source != true )
                {
                    return;
                }
                else
                {
                    Camera.ImageUrl = string.Empty;
                    Camera.ImageArray = null;
                }
            }

            Camera.Latitude = Convert.ToDouble(Latitude);
            Camera.Longitude = Convert.ToDouble(Longitude);

            IsRunning = true;
            IsEnabled = false;

            var response = await apiService.PutAsync(
                "https://ipcviewerapi.azurewebsites.net",
                "/api",
                "/Cameras",
                Camera.Id,
                Camera,
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            IsRunning = false;
            IsEnabled = true;
            if ( !response.IsSuccess )
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.ErrorEditCamera,
                    Languages.Accept);
                return;
            }

            var modifiedCamera = (Camera) response.Result;
            MainViewModel.GetInstance().Cameras.UpdateCamera(modifiedCamera);
            await App.Navigator.PopAsync();
        }

        private async void ChangeImage ()
        {
            // Inicializamos la camara
            await CrossMedia.Current.Initialize();

            // Dialogo para varias opciones
            var source = await Application.Current.MainPage.DisplayActionSheet(
                Languages.DisplayActionImage,
                Languages.Cancel,
                null,
                 Languages.FromGallery,
                 Languages.FromCamera,
                 Languages.FromUrl);

            if ( source.Equals(Languages.Cancel) )
            {
                this.file = null;
                return;
            }
            else if ( source.Equals(Languages.FromGallery) )
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
            else if ( source.Equals(Languages.FromCamera) )
            {
                this.file = await CrossMedia.Current.PickPhotoAsync();
            }
            else if ( source.Equals(Languages.FromUrl) )
            {
                MainViewModel.GetInstance().AddUrl = new AddUrlViewModel(this);
                await App.Navigator.PushAsync(new AddUrlPage());
            }

            // Si han elegido una imagen
            if ( file != null )
            {
                ImageSource = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    return stream;
                });
            }
        }

        private async void AddLocation ()
        {
            MainViewModel.GetInstance().AddLocation = new AddLocationViewModel(this);
            await App.Navigator.PushAsync(new AddLocationPage(), true);
        }

        public void SetLocation(string latitude, string longitude, byte[] imageSource)
        {
            Latitude = latitude;
            Longitude = longitude;

            if ( imageSource != null && imageSource.Length > 0 )
            {
                _imageByte = imageSource;
                ImageSource = ImageSource.FromStream(() => new MemoryStream(_imageByte));

            }
        }

        public void OnClose (string urlCamera)
        {
            if ( !string.IsNullOrEmpty(Camera.ImageUrl) )
            {
                UrlCamera = urlCamera;
            }
        }
    }
}