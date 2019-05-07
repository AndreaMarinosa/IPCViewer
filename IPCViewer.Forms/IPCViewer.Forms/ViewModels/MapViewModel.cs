using GalaSoft.MvvmLight.Command;
using IPCViewer.Common.Models;
using IPCViewer.Common.Services;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace IPCViewer.Forms.ViewModels
{
    /*
     * Using custom markers and Latitude Longitude bounds in Google Maps on Xamarin Forms
     *
     * https://javiersuarezruiz.wordpress.com/2019/04/13/novedades-en-xamarin-forms-3-6/
     */

    //TODO: mostrar la lista mediante pines personalizados
    public class MapViewModel : BaseViewModel
    {
        private bool isEnabled;
        private readonly ApiService apiService;
        private List<Camera> myCameras; // La lista original del API

        public bool IsEnabled
        {
            get => this.isEnabled;
            set => this.SetProperty(ref this.isEnabled, value);
        }


        public ICommand StreetCommand => new RelayCommand(this.Street);

        public ICommand HybridCommand => new RelayCommand(this.Hybrid);

        public ICommand SatelliteCommand => new RelayCommand(this.Satellite);

        public Map Map { get; set; }

        public MapViewModel()
        {
            Map = new Map();
            this.apiService = new ApiService();
            LoadCamerasAsync();
            Map.MoveToRegion(
               MapSpan.FromCenterAndRadius(
                   new Position(37, -122), Distance.FromMiles(1)));

            //Map.Pins.Add(new Pin
            //{

            //});
        }
        private void Street()
        {
            Map.MapType = MapType.Street;
        }


        private void Hybrid()
        {
            Map.MapType = MapType.Hybrid;
        }

        private void Satellite()
        {
            Map.MapType = MapType.Satellite;
        }

        /**
         * Cargamos todas las camaras
         */
        private async void LoadCamerasAsync()
        {
            //var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetListAsync<Camera>(
                "https://ipcviewerapi.azurewebsites.net",
                "/api",
                "/Cameras",
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Accept");
                return;
            }

            this.myCameras = (List<Camera>)response.Result;
        }
    }
}