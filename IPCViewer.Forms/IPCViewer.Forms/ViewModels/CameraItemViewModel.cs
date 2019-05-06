namespace IPCViewer.Forms.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Common.Models;
    using Views;
    using System.Windows.Input;

    /**
     * Clase para meter un comando en el modelo 'Camera'
     * Se utiliza esta nueva clase para no meter codigo interno
     * al modelo de camara, de esta manera se conserva el modelo limpio
     */
    public class CameraItemViewModel : Camera
    {
        public ICommand SelectCameraCommand => new RelayCommand(SelectCamera);

        public string LatitudeAndLongitude { get; set; }

        public CameraItemViewModel()
        {
            LatitudeAndLongitude = Latitude + ", " + Longitude;

        }
        private async void SelectCamera()
        {
            MainViewModel.GetInstance().EditCamera = new EditCameraViewModel(this);
            await App.Navigator.PushAsync(new EditCameraPage());
        }

    }
}
