using IPCViewer.Common.Models;
using IPCViewer.Common.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.GoogleMaps.Bindings;

namespace IPCViewer.Forms.ViewModels
{
    class AddLocationViewModel : BaseViewModel
    {
        private Pin _pin;
        private string _latitude;
        private string _longitude;

        // todo: localización del usuario
        private MapSpan _region =
            MapSpan.FromCenterAndRadius(
                new Position(41.655801, -0.881),
                Distance.FromKilometers(2));

        private ImageSource _imageSource;

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
                    Label = $"Pin{Pins.Count}",
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
    }
}
