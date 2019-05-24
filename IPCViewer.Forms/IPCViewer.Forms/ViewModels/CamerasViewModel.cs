using Xamarin.Forms.Internals;

namespace IPCViewer.Forms.ViewModels
{
    using Common.Models;
    using Common.Services;
    using GalaSoft.MvvmLight.Command;
    using IPCViewer.Forms.Helpers;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class CamerasViewModel : BaseViewModel
    {
        #region Private properties

        private readonly ApiService _apiService;
        private List<Camera> _myCameras; // La lista original del API

        private ObservableCollection<CameraItemViewModel> _cameras;
        private bool _isRefreshing;
        private bool _noCamerasVisible;
        private City _city;
        private ObservableCollection<Grouping<string, CameraItemViewModel>> _camerasGrouped;
        private ObservableCollection<Grouping<string, CameraItemViewModel>> _camerasGroupedSearch;
        private string _searchText;

        #endregion

        #region Public properties

        public City City { get => this._city; set => this.SetProperty(ref this._city, value); }

        public ObservableCollection<Grouping<string, CameraItemViewModel>> CamerasGrouped
        {
            get => _camerasGrouped;
            set => SetProperty(ref _camerasGrouped, value);
        }

        public ObservableCollection<Grouping<string, CameraItemViewModel>> CamerasGroupedSearch
        {
            get => _camerasGroupedSearch;
            set => SetProperty(ref _camerasGroupedSearch, value);
        }

        public ObservableCollection<CameraItemViewModel> Cameras
        {
            get => _cameras;
            set => this.SetProperty(ref _cameras, value);
        }

        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value);
        }

        public bool IsRefreshing
        {
            get => this._isRefreshing;
            set => this.SetProperty(ref this._isRefreshing, value);
        }

        public bool NoCamerasVisible
        {
            get => this._noCamerasVisible;
            set => this.SetProperty(ref this._noCamerasVisible, value);
        }

        #endregion

        #region Commands

        public ICommand SearchCommand => new RelayCommand(this.PerformSearch);
        public ICommand RefreshCommand => new RelayCommand(this.LoadCamerasAsync);

        #endregion

        #region ctor

        public CamerasViewModel ()
        {
            this._apiService = new ApiService();
            LoadCamerasAsync();
        }

        #endregion

        #region Loads
        private async void LoadCamerasAsync ()
        {
            var response = await this._apiService.GetListAsync<Camera>(
                "https://ipcviewerapi.azurewebsites.net",
                "/api",
                "/Cameras",
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            if ( !response.IsSuccess )
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.ErrorLoadCameras,
                    Languages.Accept);
                return;
            }

            this._myCameras = (List<Camera>) response.Result;
            RefreshCamerasList();
            IsRefreshing = false;
            SortCamerasList();

            NoCamerasVisible = CheckCountCameras(CamerasGrouped.Count);
        }

        #endregion

        #region Camera Functions
        public void AddCamera (Camera camera)
        {
            this._myCameras.Add(camera);
            SortCamerasList();
        }

        /**
         * Actualiza la nueva camera
         */

        public void UpdateCamera (Camera camera)
        {
            var oldCamera = _myCameras.FirstOrDefault(c => c.Id == camera.Id);

            // Eliminamos la antigua camara
            if ( oldCamera != null )
            {
                _myCameras.Remove(oldCamera);
            }

            // Aniadimos la nueva
            _myCameras.Add(camera);
            SortCamerasList();
        }

        /**
         * Elimina la camara cuyo id es pasado
         * por parametro
         */

        public void DeleteCamera (int id)
        {
            var oldCamera = _myCameras.FirstOrDefault(c => c.Id == id);
            if ( oldCamera != null )
            {
                _myCameras.Remove(oldCamera);
            }
            SortCamerasList();
        }

        #endregion

        #region Secondary Camera Functions
        private void RefreshCamerasList ()
        {

            // ObservableCollection de la Clase CameraItemViewModel -> (Camera + Comando)
            Cameras = new ObservableCollection<CameraItemViewModel>(
                _myCameras.Select(c => new CameraItemViewModel // Por cada camera se creara una nueva instancia de CameraItemViewModel
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
                    ImageFullPath = !string.IsNullOrEmpty(c.ImageUrl) ? c.ImageFullPath : "noImage" //c.ImageFullPath
                }).ToList());
        }

        private void SortCamerasList ()
        {
            //Use linq to sorty our cameras by name and then group them by the new name sort property
            var sorted =
                from camera in Cameras
                orderby camera.City.Name
                group camera by camera.NameSort into cameraGroup
                select new Grouping<string, CameraItemViewModel>(cameraGroup.Key, cameraGroup);

            //create a new collection of groups
            CamerasGrouped = new ObservableCollection<Grouping<string, CameraItemViewModel>>(sorted);
        }

        public void PerformSearch ()
        {

            if ( !string.IsNullOrWhiteSpace(this.SearchText) || SearchText != "" || !string.IsNullOrEmpty(SearchText))
            {
                SearchText = SearchText.ToLower();

                var sorted =
                    from camera in Cameras
                    where camera.Name.ToLower().Contains(SearchText)
                    orderby camera.City.Name
                    group camera by camera.NameSort into cameraGroup
                    select new Grouping<string, CameraItemViewModel>(cameraGroup.Key, cameraGroup);

                //create a new collection of groups
                CamerasGrouped = new ObservableCollection<Grouping<string, CameraItemViewModel>>(sorted);
            }
            else
            {
                SortCamerasList();
            }

            NoCamerasVisible = CheckCountCameras(CamerasGrouped.Count);
        }

        private bool CheckCountCameras (int count)
        {
            if ( count == 0 )
            {
                return true;
            }
            return false;
        }
        #endregion

    }

    /**
     * Class for grouping headers
     */
    public class Grouping<TK, T> : ObservableCollection<T>
    {
        public TK Key { get; set; }

        public Grouping (TK key, IEnumerable<T> items)
        {
            Key = key;
            foreach ( var item in items )
                this.Items.Add(item);
        }
    }
}