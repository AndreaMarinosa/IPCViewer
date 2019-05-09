﻿using IPCViewer.Common.Models;
using IPCViewer.Common.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        private int _mapClickedCount;
        private int _pinClickedCount;
        private int _selectedPinChangedCount;
        private int _infoWindowClickedCount;
        private int _infoWindowLongClickedCount;
        private string _pinDragStatus;

        private bool _animated = true;
        private MapSpan _region =
            MapSpan.FromCenterAndRadius(
                new Position(41.655801, -0.881),
                Distance.FromKilometers(2));

        private ImageSource _imageSource;


        public ObservableCollection<TileLayer> TileLayers { get; set; }

        public Command<MapClickedEventArgs> MapClickedCommand => new Command<MapClickedEventArgs>(
           args =>
           {
               //if ( TileLayers.Any() )
               //{
               //    TileLayers.Clear();
               //}
               //else
               //{
               //    Android
               //    var andString = "data:image/png;base64,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";
               //    var andImage = Convert.FromBase64String(andString.Split(",".ToCharArray(), 2)[1]);

               //    var objTile = TileLayer.FromSyncImage((x, y, zoom) => andImage);
               //    objTile.Tag = "SYNCTILE"; // Can set any object

               //    TileLayers.Add(objTile);
               //}

               MapClickedCount++;
               Pin = new Pin
               {
                   IsVisible = true,
                   Label = $"Pin{Pins.Count}",
                   Position = args.Point,
                   Type = PinType.Place
               };
               Pins?.Add(Pin);
           });

        public TakeSnapshotRequest TakeSnapshotRequest { get; } = new TakeSnapshotRequest();

        public ImageSource ImageSource
        {
            get => _imageSource;
            set => SetProperty(ref _imageSource, value);
        }

        public Command TakeSnapshotCommand => new Command(async () =>
        {
            var stream = await TakeSnapshotRequest.TakeSnapshot();
            ImageSource = ImageSource.FromStream(() => stream);
        });

        private readonly ApiService apiService;
        private List<Camera> myCameras; // La lista original del API

        public MapSpan Region
        {
            get => _region;
            set => SetProperty(ref _region, value);
        }

        public bool Animated
        {
            get => _animated;
            set => SetProperty(ref _animated, value);
        }

        public int MapClickedCount
        {
            get => _mapClickedCount;
            set => SetProperty(ref _mapClickedCount, value);
        }

        public int PinClickedCount
        {
            get => _pinClickedCount;
            set => SetProperty(ref _pinClickedCount, value);
        }

        public int SelectedPinChangedCount
        {
            get => _selectedPinChangedCount;
            set => SetProperty(ref _selectedPinChangedCount, value);
        }

        public int InfoWindowClickedCount
        {
            get => _infoWindowClickedCount;
            set => SetProperty(ref _infoWindowClickedCount, value);
        }

        public int InfoWindowLongClickedCount
        {
            get => _infoWindowLongClickedCount;
            set => SetProperty(ref _infoWindowLongClickedCount, value);
        }

        public string PinDragStatus
        {
            get => _pinDragStatus;
            set => SetProperty(ref _pinDragStatus, value);
        }

        public Pin Pin
        {
            get => _pin;
            set => SetProperty(ref _pin, value);
        }

        public ObservableCollection<Pin> Pins { get; set; }

        public Command<PinClickedEventArgs> PinClickedCommand => new Command<PinClickedEventArgs>(
            args =>
            {
                PinClickedCount++;
                Pin = args.Pin;
            });

        public Command<SelectedPinChangedEventArgs> SelectedPinChangedCommand => new Command<SelectedPinChangedEventArgs>(
            args =>
            {
                SelectedPinChangedCount++;
                Pin = args.SelectedPin;
                //Eliminar el pin
            });

        public Command<InfoWindowClickedEventArgs> InfoWindowClickedCommand => new Command<InfoWindowClickedEventArgs>(
            args =>
            {
                InfoWindowClickedCount++;
                Pin = args.Pin;
            });

        public Command<InfoWindowLongClickedEventArgs> InfoWindowLongClickedCommand => new Command<InfoWindowLongClickedEventArgs>(
            args =>
            {
                InfoWindowLongClickedCount++;
                Pin = args.Pin;
            });

        public Command<PinDragEventArgs> PinDragStartCommand => new Command<PinDragEventArgs>(
            args =>
            {
                PinDragStatus = "Start";
                Pin = args.Pin;
            });

        public Command<PinDragEventArgs> PinDraggingCommand => new Command<PinDragEventArgs>(
            args =>
            {
                PinDragStatus = "Dragging";
                Pin = args.Pin;
            });

        public Command<PinDragEventArgs> PinDragEndCommand => new Command<PinDragEventArgs>(
            args =>
            {
                PinDragStatus = "End";
                Pin = args.Pin;
            });

        public MapViewModel ()
        {
            // Creamos un nuevo api service para recoger las camaras
            this.apiService = new ApiService();

            //Cargamos las camaras y las guardamos
            LoadCamerasAsync();

            // Aniadimos los markers de las camaras
            AddMarkers();
        }

        /**
         * Cargamos todas las camaras
         */
        private async void LoadCamerasAsync ()
        {
            //var url = Application.Current.Resources["UrlAPI"].ToString();
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
                    Type = PinType.Place
                };

                Pins?.Add(pin);
            }

        }
    }
}