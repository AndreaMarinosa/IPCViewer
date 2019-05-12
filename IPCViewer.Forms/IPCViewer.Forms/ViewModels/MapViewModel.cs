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
        private readonly ApiService apiService;
        private List<Camera> myCameras; // La lista original del API
        private bool _animated = true;
        private MapSpan _region =
            MapSpan.FromCenterAndRadius(
                new Position(41.655801, -0.881),
                Distance.FromKilometers(2));

        private ImageSource _imageSource;

        // todo: poner la imagen en la label de arriba
        private ImageSource _imageUrl;

        private ObservableCollection<City> cities;
        private City city;

        public TakeSnapshotRequest TakeSnapshotRequest { get; } = new TakeSnapshotRequest();

        public City City { get => this.city; set => this.SetProperty(ref this.city, value); }

        public ObservableCollection<City> Cities { get => this.cities; set => this.SetProperty(ref this.cities, value); }

        public ObservableCollection<Pin> Pins { get; set; }

        public ImageSource ImageSource
        {
            get => _imageSource;
            set => SetProperty(ref _imageSource, value);
        }

        public ImageSource ImageUrl
        {
            get => _imageUrl;
            set => SetProperty(ref _imageUrl, value);
        }

        public Command TakeSnapshotCommand => new Command(async () =>
        {
            var stream = await TakeSnapshotRequest.TakeSnapshot();
            ImageSource = ImageSource.FromStream(() => stream);
        });

        //todo: lista de ciudades y llevar a la seleccionada
        public Command LeadMeToCommand => new Command(async () =>
        {
            
        });

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

        public Pin Pin
        {
            get => _pin;
            set => SetProperty(ref _pin, value);
        }


        public MapViewModel ()
        {
            this.apiService = new ApiService();
            LoadCamerasAsync();
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
            foreach ( var Camera in myCameras )
            {
                var pin = new Pin
                {
                    IsVisible = true,
                    Label = Camera.Name,
                    Position = new Position(Camera.Latitude, Camera.Longitude),
                    Type = PinType.Place
                };

                Pins?.Add(pin);
            }

        }

        public async void LoadCities ()
        {

            var response = await this.apiService.GetListAsync<City>(
                "https://ipcviewerapi.azurewebsites.net",
                "/api",
                "/Cities");

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
    }
}