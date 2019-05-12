namespace IPCViewer.Forms.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    using Common.Models;
    using Common.Services;
    using GalaSoft.MvvmLight.Command;
    using Xamarin.Forms;

    public class CamerasViewModel : BaseViewModel
    {
        private readonly ApiService apiService;
        private List<Camera> myCameras; // La lista original del API

        private ObservableCollection<CameraItemViewModel> cameras;
        private bool isRefreshing;

        private ObservableCollection<CityList<string, City>> cities;
        private City city;

        public City City { get => this.city; set => this.SetProperty(ref this.city, value); }

        public ObservableCollection<CityList<string, City>> CitiesGrouped { get => this.cities; set => this.SetProperty(ref this.cities, value); }

        public ObservableCollection<CameraItemViewModel> Cameras
        {
            get => cameras;
            set => this.SetProperty(ref cameras, value);
        }

        public bool IsRefreshing
        {
            get => this.isRefreshing;
            set => this.SetProperty(ref this.isRefreshing, value);
        }

        public ICommand RefreshCommand => new RelayCommand(this.LoadCamerasAsync);

        public CamerasViewModel()
        {
            this.apiService = new ApiService();
            LoadCamerasAsync();
            //LoadCities();
        }

        private async void LoadCamerasAsync()
        {
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

            this.myCameras= (List<Camera>)response.Result;
            RefreshCamerasList();
            IsRefreshing = false;

        }

        public void AddCamera(Camera camera)
        {
            

            this.myCameras.Add(camera);
            RefreshCamerasList();
        }

        /**
         * Actualiza la nueva camera
         */
        public void UpdateCamera(Camera camera)
        {
            var oldCamera = myCameras.FirstOrDefault(c => c.Id == camera.Id);

            // Eliminamos la antigua camara
            if(oldCamera != null)
            {
                myCameras.Remove(oldCamera);
            }

            // Aniadimos la nueva
            myCameras.Add(camera);
            RefreshCamerasList();
        }

        /**
         * Elimina la camara cuyo id es pasado
         * por parametro
         */
        public void DeleteCamera(int id)
        {
            var oldCamera = myCameras.FirstOrDefault(c => c.Id == id);
            if (oldCamera != null)
            {
                myCameras.Remove(oldCamera);
            }

            RefreshCamerasList();
        }

        private void RefreshCamerasList()
        {
            // ObservableCollection de la Clase CameraItemViewModel -> (Camera + Comando)
            Cameras = new ObservableCollection<CameraItemViewModel>(
                myCameras.Select(c => new CameraItemViewModel // Por cada camera se creara una nueva instancia de CameraItemViewModel
            {
                Id = c.Id,
                ImageUrl = c.ImageUrl,
                CityId = c.CityId,
                City = c.City,
                CreatedDate = c.CreatedDate,
                Latitude = c.Latitude,
                Longitude = c.Longitude,
                Comments = c.Comments,
                Name = c.Name,
                User = c.User,
                ImageFullPath = c.ImageFullPath
            }).ToList());
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

            var citiesSorted = from city in myCities
                         orderby city.Name
                         group city by city.Name into cityGroup
                         select new CityList<string, City>(cityGroup.Key, cityGroup);

            this.CitiesGrouped = new ObservableCollection<CityList<string, City>>(citiesSorted);

        }

    }
}