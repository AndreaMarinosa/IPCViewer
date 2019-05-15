using IPCViewer.Common.Models;
using IPCViewer.Common.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        private ImageSource _imageSource;
        private bool _isVisible;

        public ObservableCollection<Pin> Pins { get; set; }

        public ImageSource ImageSource
        {
            get { return _imageSource; }
            set { SetProperty(ref _imageSource, value); }
        }

        public bool IsVisible
        {
            get => _isVisible;
            set => SetProperty(ref _isVisible, value);
        }

        public MapSpan Region
        {
            get { return _region; }
            set { SetProperty(ref _region, value); }
        }

        public Pin Pin
        {
            get { return _pin; }
            set { SetProperty(ref _pin, value); }
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


        public Command<SelectedPinChangedEventArgs> SelectedPinChangedCommand
        {
            get
            {
                return new Command<SelectedPinChangedEventArgs>(
                    args =>
                    {
                        if (args.SelectedPin!=null)
                        {
                            IsVisible = true;
                            ImageSource = myCameras.FirstOrDefault(c =>
                                c.Latitude == args.SelectedPin.Position.Latitude &&
                                c.Longitude == args.SelectedPin.Position.Longitude).ImageUrl;
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
    }
}