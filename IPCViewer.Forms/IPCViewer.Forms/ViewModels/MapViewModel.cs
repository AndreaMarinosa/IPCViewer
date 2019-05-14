using IPCViewer.Common.Models;
using IPCViewer.Common.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.GoogleMaps.Bindings;

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

    //TODO: mostrar la lista mediante pines personalizados
    public class MapViewModel : BaseViewModel
    {
        private Pin _pin;
        private readonly ApiService apiService;
        private List<Camera> myCameras;
        private MapSpan _region;

        // todo: poner la imagen en la label de arriba
        private ImageSource _imageSource;


        public ObservableCollection<Pin> Pins { get; set; }

        public ImageSource ImageSource
        {
            get => _imageSource;
            set => SetProperty(ref _imageSource, value);
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
            this.apiService = new ApiService();
            LoadCamerasAsync();
            LoadLocation();
             
        }

        private async void LoadLocation()
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Medium);
            var location = await Geolocation.GetLocationAsync(request);
            Region = MapSpan.FromCenterAndRadius(
                new Position(location.Latitude, location.Longitude),
                Distance.FromKilometers(2));
        }

        /**
         * Cargamos todas las camaras
         */

        private async void LoadCamerasAsync ()
        {
            var response = await this.apiService.GetListAsync<Camera>(
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

            this.myCameras = (List<Camera>) response.Result;
            AddMarkers();
        }

        private void AddMarkers ()
        {
            foreach ( var camera in myCameras )
            {
                var pin = new Pin
                {
                    IsVisible = true,
                    Label = camera.Name,
                    Position = new Position(camera.Latitude, camera.Longitude),
                    Type = PinType.SavedPin
                };

                Pins?.Add(pin);
            }
        }

        public Command<SelectedPinChangedEventArgs> SelectedPinChangedCommand => 
            new Command<SelectedPinChangedEventArgs>(
            args => Pin = args.SelectedPin);

       
    }
}