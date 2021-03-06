﻿using GalaSoft.MvvmLight.Command;
using IPCViewer.Forms.Interfaces;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Windows.Input;
using IPCViewer.Forms.Helpers;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.GoogleMaps.Bindings;

namespace IPCViewer.Forms.ViewModels
{
    public class AddLocationViewModel : BaseViewModel
    {
        private ImageSource _imageSource;
        private byte[] _imageBytes;
        private string _latitude;
        private ILocation _location;
        private string _longitude;
        private MapType _mapType;
        private Pin _pin;
        private MapSpan _region;

        public Stream stream { get; set; }

        public ObservableCollection<Pin> Pins { get; set; }

        public ImageSource ImageSource
        {
            get => _imageSource;
            set => SetProperty(ref _imageSource, value);
        }

        public AddLocationViewModel (ILocation location)
        {
            this._location = location;
            MapType = MapType.Hybrid;
            LoadLocation();
        }
        public MapType MapType
        {
            get => this._mapType;
            set => SetProperty(ref _mapType, value);
        }

        public Pin Pin
        {
            get => _pin;
            set => SetProperty(ref _pin, value);
        }
        public MapSpan Region
        {
            get => _region;
            set => SetProperty(ref _region, value);
        }

        public ICommand GlobalCommand => new RelayCommand(this.Global);

        public ICommand SaveCommand => new RelayCommand(this.Save);

        public ICommand HybridCommand => new RelayCommand(this.Hybrid);

        public TakeSnapshotRequest TakeSnapshotRequest { get; } = new TakeSnapshotRequest();

        public Command<MapClickedEventArgs> MapClickedCommand => new Command<MapClickedEventArgs>(
            args =>
            {
                if ( Pins.Count > 1 )
                {
                    Pins.RemoveAt(1);
                }
                Pin = new Pin
                {
                    Label = "Pin",
                    Position = args.Point
                };
                Pins?.Add(Pin);
            });

        public Command<PinClickedEventArgs> PinClickedCommand => new Command<PinClickedEventArgs>(
            args => Pin = args.Pin);

        public Command TakeSnapshotCommand => new Command(async () =>
        {
            stream = await TakeSnapshotRequest.TakeSnapshot();
            //ImageSource = ImageSource.FromStream(() => stream);
            using ( MemoryStream ms = new MemoryStream() )
            {
                stream.CopyTo(ms);
                _imageBytes = ms.ToArray();
            }

            ImageSource = ImageSource.FromStream(() => new MemoryStream(_imageBytes));

        });


        private async void Save ()
        {
            if ( Pin == null )
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.ErrorSelectPin,
                    Languages.Accept);
                return;
            }

            _latitude = Pin.Position.Latitude.ToString(CultureInfo.InvariantCulture);
            _longitude = Pin.Position.Longitude.ToString(CultureInfo.InvariantCulture);

            if ( string.IsNullOrEmpty(_latitude) || string.IsNullOrEmpty(_longitude) )
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.ErrorCameraLocation,
                    Languages.Accept);
                return;
            }

            if ( ImageSource != null )
            {
                var source = await Application.Current.MainPage.DisplayAlert(
                    Languages.Alert,
                    Languages.AlertEmptyImage,
                    Languages.Accept,
                    Languages.Cancel);
                if ( !source )
                {
                    ImageSource = string.Empty;
                    _imageBytes = null;
                }
            }

            _location.SetLocation(_latitude, _longitude, _imageBytes);

            await App.Navigator.PopAsync();
        }

        private void Global () => MapType = MapType.Street;

        private void Hybrid () => MapType = MapType.Hybrid;

        private async void LoadLocation ()
        {

            var request = new GeolocationRequest(GeolocationAccuracy.Medium);
            var location = await Geolocation.GetLocationAsync(request);
            Region = MapSpan.FromCenterAndRadius(
            new Position(location.Latitude, location.Longitude),
            Distance.FromMeters(100));


            Pins.Add(new Pin
            {
                Label = Languages.OwnLocation,
                Position = new Position(location.Latitude, location.Longitude)
            });
        }
    }
}