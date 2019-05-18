using System.Net;

namespace IPCViewer.Forms.ViewModels
{
    using Common.Models;
    using Common.Services;
    using GalaSoft.MvvmLight.Command;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class CamerasViewModel : BaseViewModel
    {
        #region Private properties

        private readonly ApiService apiService;
        private List<Camera> myCameras; // La lista original del API

        private ObservableCollection<CameraItemViewModel> cameras;
        private bool isRefreshing;
        private City city;
        private ObservableCollection<Grouping<string, CameraItemViewModel>> camerasGrouped;

        #endregion

        #region Public properties

        public City City { get => this.city; set => this.SetProperty(ref this.city, value); }

        public ObservableCollection<Grouping<string, CameraItemViewModel>> CamerasGrouped
        {
            get => camerasGrouped;
            set => SetProperty(ref camerasGrouped, value);
        }

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

        #endregion

        public ICommand RefreshCommand => new RelayCommand(this.LoadCamerasAsync);

        #region ctor

        public CamerasViewModel ()
        {
            this.apiService = new ApiService();
            LoadCamerasAsync();
        }

        #endregion

        #region Loads
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
            RefreshCamerasList();
            IsRefreshing = false;

            //Use linq to sorty our cameras by name and then group them by the new name sort property
            var sorted =
                from camera in Cameras
                orderby camera.City.Name
                group camera by camera.NameSort into cameraGroup
                select new Grouping<string, CameraItemViewModel>(cameraGroup.Key, cameraGroup);

            //create a new collection of groups
            CamerasGrouped = new ObservableCollection<Grouping<string, CameraItemViewModel>>(sorted);

        }

        #endregion


        #region Camera Functions

        public void AddCamera (Camera camera)
        {
            this.myCameras.Add(camera);
            RefreshCamerasList();
        }

        /**
         * Actualiza la nueva camera
         */

        public void UpdateCamera (Camera camera)
        {
            var oldCamera = myCameras.FirstOrDefault(c => c.Id == camera.Id);

            // Eliminamos la antigua camara
            if ( oldCamera != null )
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

        public void DeleteCamera (int id)
        {
            var oldCamera = myCameras.FirstOrDefault(c => c.Id == id);
            if ( oldCamera != null )
            {
                myCameras.Remove(oldCamera);
            }
            RefreshCamerasList();
        }

        #endregion

        private void RefreshCamerasList ()
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
                    ImageFullPath =  !string.IsNullOrEmpty(c.ImageUrl ) ? c.ImageFullPath : "noImage" //c.ImageFullPath
                }).ToList());
        }
    }

    /**
     * Class for grouping headers
     */
    public class Grouping<TK, T> : ObservableCollection<T>
    {
        public TK Key { get; private set; }

        public Grouping (TK key, IEnumerable<T> items)
        {
            Key = key;
            foreach ( var item in items )
                this.Items.Add(item);
        }
    }
}