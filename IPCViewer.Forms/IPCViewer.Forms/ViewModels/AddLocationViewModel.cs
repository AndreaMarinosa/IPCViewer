using IPCViewer.Common.Models;
using IPCViewer.Common.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using IPCViewer.Forms.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.GoogleMaps.Bindings;

namespace IPCViewer.Forms.ViewModels
{
    public class AddLocationViewModel : BaseViewModel
    {
        private Pin _pin;
        private string _latitude;
        private string _longitude;
        private ILocation _location;
        private ImageSource _imageSource;

        // todo: localización del usuario
        private MapSpan _region =
            MapSpan.FromCenterAndRadius(
                new Position(41.655801, -0.881),
                Distance.FromKilometers(2));

        public AddLocationViewModel (ILocation location)
        {
            this._location = location;
        }


        public ICommand SaveCommand => new RelayCommand(this.Save);

        public ObservableCollection<Pin> Pins { get; set; }

        public TakeSnapshotRequest TakeSnapshotRequest { get; } = new TakeSnapshotRequest();

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

        public ImageSource ImageSource
        {
            get => _imageSource;
            set => SetProperty(ref _imageSource, value);
        }

        public Command<MapClickedEventArgs> MapClickedCommand => new Command<MapClickedEventArgs>(
            args =>
            {
                Pins.Clear();

                Pin = new Pin
                {
                    Label = "Pin",
                    Position = args.Point
                };
                Pins?.Add(Pin);
            });

        public Command<PinClickedEventArgs> PinClickedCommand => new Command<PinClickedEventArgs>(
           args =>
           {
               Pin = args.Pin;
           });

        public Command TakeSnapshotCommand => new Command(async () =>
        {
            var stream = await TakeSnapshotRequest.TakeSnapshot();
            ImageSource = ImageSource.FromStream(() => stream);

        });


        private async void Save ()
        {
            _latitude = Pin.Position.Latitude.ToString();
            _longitude = Pin.Position.Longitude.ToString();

            if ( string.IsNullOrEmpty(_latitude) || string.IsNullOrEmpty(_longitude) )
            {
                await Application.Current.MainPage.DisplayAlert("Error", "You must select the location.", "Accept");
                return;
            }

            if ( ImageSource != null )
            {
                var source = await Application.Current.MainPage.DisplayAlert("Important", "Do you want to save the screenshot as the camera image ? ", "Accept", "No");
                if ( !source )
                {
                    ImageSource = null;
                }
            }

            _location.SetLocation(_latitude, _longitude, ImageSource);

            // Close the last PopupPage int the PopupStack
            await App.Navigator.PopAsync();
        }
    }
}
