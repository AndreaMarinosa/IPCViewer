namespace IPCViewer.Forms.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using IPCViewer.Common.Models;
    using IPCViewer.Forms.Views;
    using System.Windows.Input;

    /**
     * Clase para meter un comando en el modelo 'Camera'
     * Se utiliza esta nueva clase para no meter codigo interno
     * al modelo de camara, de esta manera se conserva el modelo limpio
     */
    public class CameraItemViewModel : Camera
    {
        public ICommand SelectCameraCommand => new RelayCommand(this.SelectCamera);

        private async void SelectCamera()
        {
            MainViewModel.GetInstance().EditCamera = new EditCameraViewModel((Camera)this);
            await App.Navigator.PushAsync(new EditCameraPage());
        }

    }
}
