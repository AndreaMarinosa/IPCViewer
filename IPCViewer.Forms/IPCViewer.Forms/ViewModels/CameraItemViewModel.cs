namespace IPCViewer.Forms.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Common.Models;
    using Views;
    using System.Windows.Input;
    using System;

    /**
     * Clase para meter un comando en el modelo 'Camera'
     * Se utiliza esta nueva clase para no meter codigo interno
     * al modelo de camara, de esta manera se conserva el modelo limpio
     */
    public class CameraItemViewModel : Camera
    {
        public ICommand SelectCameraCommand => new RelayCommand(SelectCamera);

        public ICommand SelectCameraImageCommand => new RelayCommand(SelectCameraImage);

        private async void SelectCameraImage ()
        {
            MainViewModel.GetInstance().DisplayCamera = new DisplayViewModel(this);
            await App.Navigator.PushAsync(new DisplayCameraPage());
        }

        private async void SelectCamera()
        {
            MainViewModel.GetInstance().EditCamera = new EditCameraViewModel(this);
            await App.Navigator.PushAsync(new EditCameraPage());
        }

    }
}
