namespace IPCViewer.Forms.ViewModels
{
    using Common.Models;
    using Common.Services;
    using GalaSoft.MvvmLight.Command;
    using IPCViewer.Forms.Interfaces;
    using IPCViewer.Forms.Views;
    using System;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class EditCameraViewModel : BaseViewModel, ILocation
    {
        private readonly ApiService apiService;
        private bool isEnabled;
        private bool isRunning;
        private string latitude;
        private string longitude;

        public string Latitude
        {
            get => this.latitude;
            set => SetProperty(ref latitude, value);
        }

        public string Longitude
        {
            get => this.longitude;
            set => SetProperty(ref longitude, value);
        }

        /**
         * La camara ligada a la MainViewModel es la que se pasa por parametro
         */

        // todo: hilo actualizando la imagen
        // todo: boton que te lleve al mapa donde se muestre el pin de esta camara en concreto
        public EditCameraViewModel (Camera camera)
        {
            this.Camera = camera;
            this.apiService = new ApiService();
            this.IsEnabled = true;
            Longitude = Camera.Longitude.ToString();
            Latitude = Camera.Latitude.ToString();
        }

        private async void AddLocation ()
        {
            MainViewModel.GetInstance().AddLocation = new AddLocationViewModel(this);
            await App.Navigator.PushAsync(new AddLocationPage(), true);
        }

        private async void Delete ()
        {
            var confirm = await Application.Current.MainPage.DisplayAlert("Confirm", "Are you sure to delete the camera?", "Yes", "No");
            if ( !confirm )
            {
                return;
            }

            this.IsRunning = true;
            this.IsEnabled = false;

            var response = await this.apiService.DeleteAsync(
                "https://ipcviewerapi.azurewebsites.net",
                "/api",
                "/Cameras",
                this.Camera.Id,
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            this.IsRunning = false;
            this.IsEnabled = true;

            if ( !response.IsSuccess )
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Accept");
                return;
            }

            MainViewModel.GetInstance().Cameras.DeleteCamera(this.Camera.Id);
            await App.Navigator.PopAsync();
        }

        private async void DisplayCameraAsync ()
        {
            MainViewModel.GetInstance().DisplayCamera = new DisplayViewModel(Camera);
            await App.Navigator.PushAsync(new DisplayCameraPage());
        }

        private async void Save ()
        {
            // todo: more alerts
            if ( string.IsNullOrEmpty(this.Camera.Name) )
            {
                await Application.Current.MainPage.DisplayAlert("Error", "You must enter a camera name.", "Accept");
                return;
            }

            // Todo: other alerts

            this.IsRunning = true;
            this.IsEnabled = false;

            Camera.Latitude = Convert.ToDouble(Latitude);
            Camera.Longitude = Convert.ToDouble(Longitude);

            var response = await this.apiService.PutAsync(
                "https://ipcviewerapi.azurewebsites.net",
                "/api",
                "/Cameras",
                this.Camera.Id,
                this.Camera,
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            this.IsRunning = false;
            this.IsEnabled = true;
            if ( !response.IsSuccess )
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Accept");
                return;
            }

            var modifiedCamera = (Camera) response.Result;
            MainViewModel.GetInstance().Cameras.UpdateCamera(modifiedCamera);
            await App.Navigator.PopAsync();
        }

        public void SetLocation (string longitude, string latitude, ImageSource imageSource)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public ICommand AddLocationCommand => new RelayCommand(this.AddLocation);

        public Camera Camera { get; set; }

        public ICommand DeleteCommand => new RelayCommand(this.Delete);

        public bool IsEnabled
        {
            get => this.isEnabled;
            set => this.SetProperty(ref this.isEnabled, value);
        }

        public bool IsRunning
        {
            get => this.isRunning;
            set => this.SetProperty(ref this.isRunning, value);
        }


        public ICommand SaveCommand => new RelayCommand(this.Save);

        public ICommand TapCommand => new RelayCommand(this.DisplayCameraAsync);
    }
}