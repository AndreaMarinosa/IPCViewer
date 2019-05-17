using IPCViewer.Common.Models;
using IPCViewer.Common.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace IPCViewer.Forms.ViewModels
{
    /*
     * Using custom markers and Latitude Longitude bounds in Google Maps on Xamarin Forms
     *
     * https://javiersuarezruiz.wordpress.com/2019/04/13/novedades-en-xamarin-forms-3-6/
     *
     * https://xamarin.in.th/xamarin-links-collection-components-controls-plugins-samples-update-consistently-1ceda3c1ba35
     *
     * https://awesomeopensource.com/project/jsuarezruiz/awesome-xamarin-forms
     *
     * https://xamarinlatino.com/crear-un-pin-personalizado-para-mis-mapas-en-xamarin-forms-f4d62ada5e30
     */

    public class MapViewModel : BaseViewModel
    {
        private Pin _pin;
        private readonly ApiService _apiService;
        private List<Camera> _myCameras;
        private MapSpan _region;
        private ImageSource _imageSource;
        private bool _isVisible;

        public Location UserLocation { get; set; }
        public ObservableCollection<Pin> Pins { get; set; }

        public ImageSource ImageSource
        {
            get => _imageSource;
            set => SetProperty(ref _imageSource, value);
        }

        public bool IsVisible
        {
            get => _isVisible;
            set => SetProperty(ref _isVisible, value);
        }

        public MapSpan Region
        {
            get => _region;
            set => SetProperty(ref _region, value);
        }

        public Pin Pin
        {
            get => _pin;
            set => SetProperty(ref _pin, value);
        }

        public MapViewModel ()
        {
            this._apiService = new ApiService();
            LoadCamerasAsync();
            LoadLocation();

        }

        public MapViewModel (Camera camera)
        {
            this._apiService = new ApiService();
            LoadCamerasAsync();
            LoadLocationCameraAsync(camera);

        }

        private async void LoadLocationCameraAsync (Camera camera)
        {
            bool success =  await RequestLocation();

            if ( success )
            {
                Region = MapSpan.FromCenterAndRadius(
                new Position(camera.Latitude, camera.Longitude),
                Distance.FromKilometers(2));
            }
        }
       
        private async void LoadLocation ()
        {
            bool success = await RequestLocation();

            if ( success )
            {
                Region = MapSpan.FromCenterAndRadius(
                new Position(UserLocation.Latitude, UserLocation.Longitude),
                Distance.FromKilometers(2));
            }
        }

        /**
         * Cargamos todas las camaras
         */

        private async void LoadCamerasAsync ()
        {
            var response = await this._apiService.GetListAsync<Camera>(
                "https://ipcviewerapi.azurewebsites.net",
                "/api",
                "/Cameras",
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            if ( !response.IsSuccess )
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Accept");
                return;
            }

            this._myCameras = (List<Camera>) response.Result;
            AddMarkers();
        }

        /***
         * Añade los pines al mapa
         */
        private void AddMarkers ()
        {
            foreach ( var camera in _myCameras )
            {
                var pin = new Pin
                {
                    IsVisible = true,
                    Label = camera.Name,
                    Position = new Position(camera.Latitude, camera.Longitude),
                    Type = PinType.SavedPin,
                    Icon = BitmapDescriptorFactory.FromBundle("type" + 2)
                };

                Pins?.Add(pin);
            }
        }

        /***
         * Cuando pulse un pin, se le hará visible el layout que contiene los valores de ese pin además de la imagen que coincida con la
         * posicion de la camara y el pin
         */
        public Command<SelectedPinChangedEventArgs> SelectedPinChangedCommand
        {
            get
            {
                return new Command<SelectedPinChangedEventArgs>(
                    args =>
                    {
                        if ( args.SelectedPin != null && args.SelectedPin.Label != "Your position" )
                        {
                            IsVisible = true;
                            ImageSource = _myCameras.FirstOrDefault(c =>
                                c.Latitude == args.SelectedPin.Position.Latitude &&
                                c.Longitude == args.SelectedPin.Position.Longitude).ImageFullPath;
                        }
                        else
                        {
                            IsVisible = false;
                            ImageSource = string.Empty;
                        }
                        Pin = args.SelectedPin;
                    });
            }
        }
        private async Task<bool> RequestLocation ()
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Medium);
            var location = await Geolocation.GetLocationAsync(request);
            UserLocation = location;

            if ( UserLocation == null )
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Failure to receive the user's location",
                    "Accept");
                return false;
            }

            Pins.Add(new Pin
            {
                Label = "Your position",
                Position = new Position(UserLocation.Latitude, UserLocation.Longitude),
            });

            return true;
        }
    }
}