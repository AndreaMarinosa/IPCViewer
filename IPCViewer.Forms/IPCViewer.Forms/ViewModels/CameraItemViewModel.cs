using System;

namespace IPCViewer.Forms.ViewModels
{
    using Common.Models;
    using GalaSoft.MvvmLight.Command;
    using System.Windows.Input;
    using Views;
    using Xamarin.Forms;

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
            await App.Navigator.PushAsync(new DisplayCameraPage(), true);
        }

        private async void SelectCameraEdit ()
        {
            MainViewModel.GetInstance().EditCamera = new EditCameraViewModel(this);
            await App.Navigator.PushAsync(new EditCameraPage());
        }
        async void SelectCamera ()
        {
            var source = await Application.Current.MainPage.DisplayActionSheet(
                "What do you want to do?", "Cancel", null,
                "Edit camera", "Delete Camera");

            switch ( source )
            {
                case "Cancel":
                    {
                        return;
                    }
                case "Edit camera":
                    {
                        SelectCameraEdit();
                        break;
                    }
                case "View Camera":
                    {
                        SelectCameraImage();
                        break;
                    }
            }
        }
    }
}